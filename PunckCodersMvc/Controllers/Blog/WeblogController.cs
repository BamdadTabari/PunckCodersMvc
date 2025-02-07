using DataProvider.Assistant.Pagination;
using DataProvider.EntityFramework.Entities.Blog;
using DataProvider.EntityFramework.Repository;
using DataProvider.Models.Query.Blog.PostCategory;
using DataProvider.Models.Result.Blog.Post;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using PunckCodersMvc.Configs;

namespace PunckCodersMvc.Controllers.Blog;
public class WeblogController : Controller
{
    private readonly IMemoryCache _memoryCache;
    private readonly CacheOptions _cacheOptions;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;
    private const string CacheKey = "weblog";
    private const string CacheLockKey = "weblog_lock";

    public WeblogController(IMemoryCache memoryCache, IOptions<CacheOptions> cacheOptions, IUnitOfWork unitOfWork, ILogger logger)
    {
        _memoryCache = memoryCache;
        _cacheOptions = cacheOptions.Value;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    [HttpGet]
    [Route("get-by-id")]
    public async Task<IActionResult> GetPostCategory([FromQuery] GetPostCategoryQuery getPostCategoryQuery)
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

                    if (result == null) return NotFound("Post Category not found.");

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
                return await GetPostCategory(getPostCategoryQuery);
            }
        }
        return Ok(result);
    }

    [HttpGet]
    [Route("get-by-filter")]
    public IActionResult GetPaginatedPostCategory([FromQuery] GetPagedPostCategoryQuery getPagedPostCategoryQuery)
    {
        var result = _unitOfWork.PostCategoryRepo.GetPaginated(getPagedPostCategoryQuery);
        return Ok(result);
    }

    [HttpGet]
    [Route("get-by-id")]
    public async Task<IActionResult> GetPost([FromQuery] GetPostQuery getPostQuery)
    {
        string cacheKey = $"{CacheKey}_{getPostQuery.PostId}";
        string lockKey = $"{CacheLockKey}_{getPostQuery.PostId}";

        if (!_memoryCache.TryGetValue(cacheKey, out PostResult? result))
        {
            // Lock mechanism to prevent cache stampede
            if (!_memoryCache.TryGetValue(lockKey, out _))
            {
                try
                {
                    _memoryCache.Set(lockKey, true, _cacheOptions.LockExpiration);
                    var post = await _unitOfWork.PostRepo.GetByIdAsync(getPostQuery.PostId);
                    if (post == null) return NotFound("Post not found.");

                    // Increment view count and update database
                    result.ViewCount += 1;
                    _unitOfWork.PostRepo.Update(post);
                    await _unitOfWork.CommitAsync();


                    var userLiked = await _unitOfWork.PostLikeRepo.
                        Get(_unitOfWork.UserRepo.GetUser(User.Identity.Name).Id,
                        getPostQuery.PostId);
                    result = new PostResult()
                    {
                        PostCommentResults = post.PostComments.Select(x => new PostCommentResult()).ToList(),
                        Id = post.Id,
                        Content = post.Content,
                        CreatedAt = post.CreatedAt,
                        ViewCount = post.ViewCount,
                        Author = post.Author.Username,
                        Title = post.Title,
                        ShortDescription = post.ShortDescription,
                        Image = post.Image,
                        PostCategoryId = post.PostCategoryId,
                        PostCategoryName = post.PostCategory.Name,
                        IsUserLiked = userLiked != null ? true : false
                    };

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
                return await GetPost(getPostQuery);
            }
        }
        return Ok(result);
    }

    [HttpGet]
    [Route("get-by-filter")]
    public IActionResult GetPaginatedPost([FromQuery] GetPagedPostQuery getPagedPostQuery)
    {
        var result = _unitOfWork.PostRepo.GetPaginated(getPagedPostQuery);
        return Ok(result);
    }
}
