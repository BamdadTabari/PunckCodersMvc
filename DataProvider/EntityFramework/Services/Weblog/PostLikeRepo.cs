using DataProvider.Assistant.Pagination;
using DataProvider.EntityFramework.Configs;
using DataProvider.EntityFramework.Entities.Blog;
using DataProvider.EntityFramework.Repository;
using DataProvider.Models.Query.Blog.PostCategory;
using Microsoft.EntityFrameworkCore;

namespace DataProvider.EntityFramework.Services.Weblog;
public interface IPostLikeRepo : IRepository<PostLike>
{
    Task<PostLike> Get(int userId, int postId);
    int Count(int postId);
}

public class PostLikeRepo: Repository<PostLike>, IPostLikeRepo
{
    private readonly IQueryable<PostLike> _queryable;

    private readonly Serilog.ILogger _logger;
    public PostLikeRepo(AppDbContext context, Serilog.ILogger logger) : base(context)
    {
        _queryable = DbContext.Set<PostLike>();
        _logger = logger;
    }

    public int Count(int postId)
    {
        return _queryable.Count(x => x.PostId == postId);
    }

    public async Task<PostLike> Get(int userId, int postId)
    {
        try
        {
            return await _queryable.SingleOrDefaultAsync(x => x.UserId == userId && x.PostId == postId) ?? new PostLike();
        }
        catch
        {
            _logger.Error("Error in GetPostLike");
            return new PostLike();
        }
    }
}