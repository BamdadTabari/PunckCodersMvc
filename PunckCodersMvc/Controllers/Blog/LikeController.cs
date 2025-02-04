using DataProvider.EntityFramework.Entities.Blog;
using DataProvider.EntityFramework.Repository;
using DataProvider.Models.Command.Blog.Comment;
using DataProvider.Models.Command.Blog.Like;
using DataProvider.Models.Query.Blog.PostComment;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using PunckCodersMvc.Configs;

namespace PunckCodersMvc.Controllers.Blog;
public class LikeController : Controller
{
    private readonly IMemoryCache _memoryCache;
    private readonly CacheOptions _cacheOptions;
    private readonly IUnitOfWork _unitOfWork;
    private const string CacheKey = "PostLike";
    private const string CacheLockKey = "PostLike_Lock";
    private readonly ILogger _logger;
    private static readonly object _cacheLock = new(); // Lock object for caching

    public LikeController(IMemoryCache memoryCache, IOptions<CacheOptions> cacheOptions, IUnitOfWork unitOfWork, ILogger logger)
    {
        _memoryCache = memoryCache;
        _cacheOptions = cacheOptions.Value;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> Create([FromForm] CreateLikeCommand createLikeCommand)
    {
        try
        {
            var entity = new PostLike
            {
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsDeleted = false,
                UserId =( await _unitOfWork.UserRepo.GetUser(User.Identity.Name)).Id,
                PostId = createLikeCommand.PostId,
            };

            await _unitOfWork.PostLikeRepo.AddAsync(entity);
            if (!await _unitOfWork.CommitAsync())
                return BadRequest("Error occurred while creating the Like.");

            lock (_cacheLock)
            {
                CacheManager.ClearKeysByPrefix(_memoryCache, CacheKey);
            }

            return Ok("Like created successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error on create Like at {Time}", DateTime.UtcNow);
            return BadRequest("Error On Create Like");
        }
    }

    [HttpPut]
    [Route("edit")]
    public async Task<IActionResult> Edit([FromForm] EditCommentCommand editCommentCommand)
    {
        try
        {
            var entity = await _unitOfWork.PostCommentRepo.GetByIdAsync(editCommentCommand.CommentId);
            entity.UpdatedAt = DateTime.Now;
            entity.Text = editCommentCommand.Text;

            _unitOfWork.PostCommentRepo.Update(entity);
            if (!await _unitOfWork.CommitAsync())
                return BadRequest("Error occurred while updating the comment.");

            lock (_cacheLock)
            {
                CacheManager.ClearKeysByPrefix(_memoryCache, CacheKey);
            }

            return Ok("Comment updated successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error on edit comment at {Time}", DateTime.UtcNow);
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    [Route("get-by-id")]
    public async Task<IActionResult> Get([FromQuery] GetPostCommentQuery getPotCommentQuery)
    {
        string cacheKey = $"{CacheKey}_{getPotCommentQuery.PostCommentId}";
        string lockKey = $"{CacheLockKey}_{getPotCommentQuery.PostCommentId}";

        if (!_memoryCache.TryGetValue(cacheKey, out PostComment? result))
        {
            // Lock mechanism to prevent cache stampede
            if (!_memoryCache.TryGetValue(lockKey, out _))
            {
                try
                {
                    _memoryCache.Set(lockKey, true, _cacheOptions.LockExpiration);

                    result = await _unitOfWork.PostCommentRepo.GetByIdAsync(getPotCommentQuery.PostCommentId);

                    if (result == null) return NotFound("Post comment not found.");

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
                return await Get(getPotCommentQuery);
            }
        }

        return Ok(result);
    }

    [HttpGet]
    [Route("get-by-filter")]
    public IActionResult GetPaginated([FromQuery] GetPagedPostCommentQuery getPagedPostCommentQuery)
    {
        var result = _unitOfWork.PostCommentRepo.GetPaginated(getPagedPostCommentQuery);
        return Ok(result);
    }

    [HttpDelete]
    [Route("delete")]
    public async Task<IActionResult> Delete([FromForm] DeleteCommentCommand deleteCommentCommand)
    {
        try
        {
            var entity = await _unitOfWork.PostCommentRepo.GetByIdAsync(deleteCommentCommand.CommentId);
            if (entity == null) return NotFound("Post comment not found.");

            entity.IsDeleted = true;
            _unitOfWork.PostCommentRepo.Update(entity);

            if (!await _unitOfWork.CommitAsync())
                return BadRequest("Error occurred while deleting the comment.");

            lock (_cacheLock)
            {
                CacheManager.ClearKeysByPrefix(_memoryCache, CacheKey);
            }

            return Ok("comment deleted successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error on Delete comment at {Time}", DateTime.UtcNow);
            return BadRequest(ex.Message);
        }
    }
}
