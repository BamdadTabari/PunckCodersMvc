using DataProvider.Assistant.Pagination;
using DataProvider.EntityFramework.Configs;
using DataProvider.EntityFramework.Entities.Blog;
using DataProvider.EntityFramework.Entities.Identity;
using DataProvider.EntityFramework.Repository;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace DataProvider.EntityFramework.Services.Identity;

public interface IUserRoleRepo : IRepository<UserRole>
{
    IEnumerable<UserRole> GetUserRolesByUserId(int userId);
    Task<PaginatedList<UserRole>> GetByRoleId(int roleId);
}
public class UserRoleRepo : Repository<UserRole>, IUserRoleRepo
{
    private readonly IQueryable<UserRole> _queryable;

    private readonly ILogger _logger;

    public UserRoleRepo(AppDbContext context, ILogger logger) : base(context)
    {
        _queryable = DbContext.Set<UserRole>();
        _logger = logger;
    }

    public Task<PaginatedList<UserRole>> GetByRoleId(int roleId)
    {
        try
        {
            var query = _queryable.Where(x => x.Id == filter.LastId).AsNoTracking().ApplyFilter(filter).ApplySort(filter.SortBy);
            var dataTotalCount = _queryable.Count();
            return new PaginatedList<PostComment>([.. query], dataTotalCount, filter.Page, filter.PageSize);
        }
        catch
        {
            _logger.Error("Error in GetPaginatedPostComment");
            return new PaginatedList<PostComment>(new List<PostComment>(), 0, filter.Page, filter.PageSize);
        }
    }

    public IEnumerable<UserRole> GetUserRolesByUserId(int userId)
    {
        try
        {
            return _queryable.Include(i => i.Role).Where(x => x.UserId == userId);
        }
        catch (Exception ex)
        {
            _logger.Error("Error in Get UserRole By UserId", ex);
            return new List<UserRole>().AsEnumerable();
        }
    }
}
