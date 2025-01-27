using DataProvider.Assistant.Pagination;
using DataProvider.EntityFramework.Entities.Blog;
using DataProvider.EntityFramework.Repository;
using DataProvider.Models.Command.Blog.PostCategory;
using DataProvider.Models.Query.Blog.PostCategory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using PunckCodersMvc.Configs;
namespace PunkCodersMvc.Controllers.Admin;

[ApiController]
public class PostCategoryController : Controller
{
    private readonly IMemoryCache _memoryCache;
    private readonly CacheOptions _cacheOptions;
    private readonly IUnitOfWork _unitOfWork;
    private const string CacheKey = "PostCategory";
    private const string CacheLockKey = "PostCategory_Lock";
    private readonly ILogger _logger;
    private static readonly object _cacheLock = new(); // Lock object for caching

    public PostCategoryController(IMemoryCache memoryCache, IOptions<CacheOptions> cacheOptions, IUnitOfWork unitOfWork, ILogger logger)
    {
        _memoryCache = memoryCache;
        _cacheOptions = cacheOptions.Value;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> Create([FromForm] CreatePostCategoryCommand createPostCategoryCommand)
    {
        try
        {
            if (await _unitOfWork.PostCategoryRepo.AnyAsync(createPostCategoryCommand.Name))
                return BadRequest("PostCategory already exists");

            var entity = new PostCategory
            {
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsDeleted = false,
                Name = createPostCategoryCommand.Name,
            };

            await _unitOfWork.PostCategoryRepo.AddAsync(entity);
            if (!await _unitOfWork.CommitAsync())
                return BadRequest("Error occurred while creating the category.");

            lock (_cacheLock)
            {
                CacheManager.ClearKeysByPrefix(_memoryCache, CacheKey);
            }

            return Ok("Post category created successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error on create post category at {Time}", DateTime.UtcNow);
            return BadRequest("Error On Create PostCategory");
        }
    }

    [HttpPut]
    [Route("edit")]
    public async Task<IActionResult> Edit([FromForm] EditPostCategoryCommand editPostCategoryCommand)
    {
        try
        {
            if (await _unitOfWork.PostCategoryRepo.AnyAsync(editPostCategoryCommand.Name))
                return BadRequest("PostCategory already exists");

            var entity = await _unitOfWork.PostCategoryRepo.GetByIdAsync(editPostCategoryCommand.PostCategoryId);
            entity.UpdatedAt = DateTime.Now;
            entity.Name = editPostCategoryCommand.Name;

            _unitOfWork.PostCategoryRepo.Update(entity);
            if (!await _unitOfWork.CommitAsync())
                return BadRequest("Error occurred while updating the category.");

            lock (_cacheLock)
            {
                CacheManager.ClearKeysByPrefix(_memoryCache, CacheKey);
            }

            return Ok("Post category updated successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error on edit post category at {Time}", DateTime.UtcNow);
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    [Route("get-by-id")]
    public async Task<IActionResult> Get([FromQuery] GetPostCategoryQuery getPostCategoryQuery)
    {
        string cacheKey = $"{CacheKey}_{getPostCategoryQuery.PostCategoryId}";
        string lockKey = $"{CacheLockKey}_{getPostCategoryQuery.PostCategoryId}";

        if (!_memoryCache.TryGetValue(cacheKey, out PostCategory? result))
        {
            // Lock mechanism to prevent cache stampede
            if (!_memoryCache.TryGetValue(lockKey, out _))
            {
                try
                {
                    _memoryCache.Set(lockKey, true, _cacheOptions.LockExpiration);

                    result = await _unitOfWork.PostCategoryRepo.GetByIdAsync(getPostCategoryQuery.PostCategoryId);

                    if (result == null) return NotFound("Post category not found.");

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
                return await Get(getPostCategoryQuery);
            }
        }

        return Ok(result);
    }

    [HttpGet]
    [Route("get-by-filter")]
    public IActionResult GetPaginated([FromQuery] GetPagedPostCategoryQuery getPagedPostCategoryQuery)
    {
        var result = _unitOfWork.PostCategoryRepo.GetPaginated(getPagedPostCategoryQuery);
        return Ok(result);
    }

    [HttpDelete]
    [Route("delete")]
    public async Task<IActionResult> Delete([FromForm] DeletePostCategoryCommand deletePostCategoryCommand)
    {
        try
        {
            var entity = await _unitOfWork.PostCategoryRepo.GetByIdAsync(deletePostCategoryCommand.PostCategoryId);
            if (entity == null) return NotFound("Post category not found.");

            entity.IsDeleted = true;
            _unitOfWork.PostCategoryRepo.Update(entity);

            if (entity.Posts != null)
            {
                foreach (var post in entity.Posts)
                {
                    post.IsDeleted = true;
                    _unitOfWork.PostRepo.Update(post);

                    if (post.PostComments != null)
                    {
                        foreach (var comment in post.PostComments)
                        {
                            comment.IsDeleted = true;
                            _unitOfWork.PostCommentRepo.Update(comment);
                        }
                    }
                }
            }

            if (!await _unitOfWork.CommitAsync())
                return BadRequest("Error occurred while deleting the category.");

            lock (_cacheLock)
            {
                CacheManager.ClearKeysByPrefix(_memoryCache, CacheKey);
            }

            return Ok("Post category deleted successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error on Delete post category at {Time}", DateTime.UtcNow);
            return BadRequest(ex.Message);
        }
    }
}
