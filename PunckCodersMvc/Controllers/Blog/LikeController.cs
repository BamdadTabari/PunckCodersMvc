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
                UserId = (await _unitOfWork.UserRepo.GetUser(User.Identity.Name)).Id,
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

    [HttpDelete]
    [Route("delete")]
    public async Task<IActionResult> Delete([FromForm] DeleteLikeCommand deleteLikeCommand)
    {
        try
        {
            var entity = await _unitOfWork.PostLikeRepo.Get(deleteLikeCommand.UserId, deleteLikeCommand.PostId);
            if (entity == null) return NotFound("Like comment not found.");

            entity.IsDeleted = true;
            _unitOfWork.PostLikeRepo.Update(entity);

            if (!await _unitOfWork.CommitAsync())
                return BadRequest("Error occurred while deleting the Like.");

            lock (_cacheLock)
            {
                CacheManager.ClearKeysByPrefix(_memoryCache, CacheKey);
            }

            return Ok("Like deleted successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error on Delete Like at {Time}", DateTime.UtcNow);
            return BadRequest(ex.Message);
        }
    }
}
