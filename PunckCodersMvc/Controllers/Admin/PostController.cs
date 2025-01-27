using DataProvider.Assistant.Pagination;
using DataProvider.EntityFramework.Entities.Blog;
using DataProvider.EntityFramework.Repository;
using DataProvider.Models.Command.Blog.Post;
using DataProvider.Models.Query.Blog.PostCategory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using PunckCodersMvc.Configs;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace PunckCodersMvc.Controllers.Admin;
public class PostController : Controller
{
    private readonly IMemoryCache _memoryCache;
    private readonly CacheOptions _cacheOptions;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;
    private readonly JwtTokenService _tokenService;
    private const string CacheKey = "Post";
    private const string CacheLockKey = "Post_Lock";

    public PostController(IMemoryCache memoryCache, IOptions<CacheOptions> cacheOptions, IUnitOfWork unitOfWork, ILogger logger, JwtTokenService tokenService)
    {
        _memoryCache = memoryCache;
        _cacheOptions = cacheOptions.Value;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _tokenService = tokenService;
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> Create([FromForm] CreatePostCommand createPostCommand)
    {
        try
        {
            // Check if post title already exists
            if (await _unitOfWork.PostRepo.AnyAsync(createPostCommand.Title))
                return BadRequest("Post already exists");

            var uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "files");
            Directory.CreateDirectory(uploadsFolderPath);

            var imageFileName = $"{Guid.NewGuid()}{Path.GetExtension(createPostCommand.Image.FileName)}";
            var imagePath = Path.Combine(uploadsFolderPath, imageFileName);

            using (var stream = createPostCommand.Image.OpenReadStream())
            {
                using var image = await Image.LoadAsync(stream);

                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Mode = ResizeMode.Max,
                    Size = new Size(800, 600)
                }));

                await image.SaveAsync(imagePath, new SixLabors.ImageSharp.Formats.Jpeg.JpegEncoder { Quality = 75 });
            }

            var entity = new Post
            {
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsDeleted = false,
                Title = createPostCommand.Title,
                Content = createPostCommand.Content,
                Image = imagePath,
                AuthorId = int.Parse(_tokenService.GetUserIdFromClaims(User)),
                IsPublished = createPostCommand.IsPublished,
                ShortDescription = createPostCommand.ShortDescription,
            };

            await _unitOfWork.PostRepo.AddAsync(entity);

            if (!await _unitOfWork.CommitAsync())
                return BadRequest("Error occurred while creating the post.");

            CacheManager.ClearKeysByPrefix(_memoryCache, CacheKey);

            return Ok("Post created successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error on creating post at {Time}", DateTime.UtcNow);
            return BadRequest("Error on creating post.");
        }
    }

    [HttpGet]
    [Route("get-by-id")]
    public async Task<IActionResult> Get([FromQuery] GetPostQuery getPostQuery)
    {
        string cacheKey = $"{CacheKey}_{getPostQuery.PostId}";
        string lockKey = $"{CacheLockKey}_{getPostQuery.PostId}";

        if (!_memoryCache.TryGetValue(cacheKey, out Post? result))
        {
            // Lock mechanism to prevent cache stampede
            if (!_memoryCache.TryGetValue(lockKey, out _))
            {
                try
                {
                    _memoryCache.Set(lockKey, true, _cacheOptions.LockExpiration);

                    result = await _unitOfWork.PostRepo.GetByIdAsync(getPostQuery.PostId);

                    if (result == null) return NotFound("Post not found.");

                    _memoryCache.Set(cacheKey, result, new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = _cacheOptions.AbsoluteExpiration,
                        SlidingExpiration = _cacheOptions.SlidingExpiration
                    });
                }
                finally
                {
                    _memoryCache.Remove(lockKey);
                }
            }
            else
            {
                // Wait and retry if lock is active
                await Task.Delay(100);
                return await Get(getPostQuery);
            }
        }

        return Ok(result);
    }

    [HttpGet]
    [Route("get-by-filter")]
    public IActionResult GetPaginated([FromQuery] GetPagedPostQuery getPagedPostQuery)
    {
        var  result = _unitOfWork.PostRepo.GetPaginated(getPagedPostQuery);
        return Ok(result);
    }
}
