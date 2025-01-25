using DataProvider.Assistant.Pagination;
using DataProvider.EntityFramework.Configs;
using DataProvider.EntityFramework.Entities.Blog;
using DataProvider.EntityFramework.Entities.Identity;
using DataProvider.EntityFramework.Extensions.Identity;
using DataProvider.EntityFramework.Repository;
using DataProvider.Models.Query.Identity.Role;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace DataProvider.EntityFramework.Services.Identity;
public interface IRoleRepo : IRepository<Role>
{
    Task<Role> GetRole(string title);
    Task<Role> GetRole(int id);
    Task<bool> AnyExist(string title);
    PaginatedList<Role> GetPaginated(GetPagedRoleQuery filter);
}
public class RoleRepo : Repository<Role>, IRoleRepo
{
    private readonly IQueryable<Role> _queryable;

    private readonly Serilog.ILogger _logger;

    public RoleRepo(AppDbContext context, Serilog.ILogger logger) : base(context)
    {
        _queryable = DbContext.Set<Role>();
        _logger = logger;
    }

    public async Task<bool> AnyExist(string title)
    {
        try
        {
            return await _queryable.AnyAsync(x => x.Title == title);
        }
        catch (Exception ex)
        {
            _logger.Error("Error in User AnyAsync", ex);
            return await Task.FromResult(false);
        }
    }

    public PaginatedList<Role> GetPaginated(GetPagedRoleQuery filter)
    {
        try
        {
            var query = _queryable.Where(x => x.Id > filter.LastId).AsNoTracking().ApplyFilter(filter).ApplySort(filter.SortBy);
            var dataTotalCount = _queryable.Count();
            return new PaginatedList<Role>([.. query], dataTotalCount, filter.Page, filter.PageSize);
        }
        catch
        {
            _logger.Error("Error in GetPaginatedPost");
            return new PaginatedList<Role>([], 0, filter.Page, filter.PageSize);
        }
    }

    public async Task<Role> GetRole(string title)
    {
        try
        {
            return await _queryable.SingleOrDefaultAsync(x => x.Title == title) ?? new Role();
        }
        catch (Exception ex)
        {
            _logger.Error("Error in Get Role By title", ex);
            return new Role();
        }
    }
    public async Task<Role> GetRole(int id)
    {
        try
        {
            return await _queryable.SingleOrDefaultAsync(x => x.Id == id) ?? new Role();
        }
        catch (Exception ex)
        {
            _logger.Error("Error in Get Role By Name", ex);
            return new Role();
        }
    }
}
