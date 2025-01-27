using DataProvider.Assistant.Pagination;
using DataProvider.EntityFramework.Configs;
using DataProvider.EntityFramework.Entities.Blog;
using DataProvider.EntityFramework.Extensions.Blog;
using DataProvider.EntityFramework.Repository;
using DataProvider.Models.Query.Blog.PostComment;
using Microsoft.EntityFrameworkCore;

namespace DataProvider.EntityFramework.Services.Weblog;
public interface IPostCommentRepo : IRepository<PostComment>
{
    Task<PostComment> GetByIdAsync(int id);
    PaginatedList<PostComment> GetPaginated(GetPagedPostCommentQuery filter);
    Task<List<PostComment>> GetAll();
}

public class PostCommentRepo : Repository<PostComment>, IPostCommentRepo
{
    private readonly IQueryable<PostComment> _queryable;

    private readonly Serilog.ILogger _logger;

    public PostCommentRepo(AppDbContext context, Serilog.ILogger logger) : base(context)
    {
        _queryable = DbContext.Set<PostComment>();
        _logger = logger;
    }

    public async Task<List<PostComment>> GetAll()
    {
        try
        {
            return await _queryable.ToListAsync();
        }
        catch
        {

            _logger.Error("Error in GetAll");
            return [];
        }
    }

    public async Task<PostComment> GetByIdAsync(int id)
    {
        try
        {
            return await _queryable.SingleOrDefaultAsync(x => x.Id == id && x.IsDeleted == false) ?? new PostComment();
        }
        catch
        {
            _logger.Error("Error in GetByPostCommentIdAsync");
            return new PostComment();
        }
    }

    public PaginatedList<PostComment> GetPaginated(GetPagedPostCommentQuery filter)
    {
        try
        {
            var query = _queryable.Where(x => x.Id == filter.LastId).Take(filter.PageSize).AsNoTracking().ApplyFilter(filter).ApplySort(filter.SortBy);
            var dataTotalCount = _queryable.Count();
            return new PaginatedList<PostComment>([.. query], dataTotalCount, filter.Page, filter.PageSize);
        }
        catch
        {
            _logger.Error("Error in GetPaginatedPostComment");
            return new PaginatedList<PostComment>(new List<PostComment>(), 0, filter.Page, filter.PageSize);
        }
    }
}