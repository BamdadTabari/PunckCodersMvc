using DataProvider.EntityFramework.Configs;
using DataProvider.EntityFramework.Entities.Identity;
using DataProvider.EntityFramework.Repository;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace DataProvider.EntityFramework.Services.Identity;
public interface IRoleRepo : IRepository<Role>
{
    Task<Role> GetRole(string title);
    Task<Role> GetRole(int id);
    Task<bool> AnyExist(string title);
}
public class RoleRepo : Repository<Role>, IRoleRepo
{
    private readonly IQueryable<Role> _queryable;

    private readonly ILogger _logger;

    public RoleRepo(AppDbContext context, ILogger logger) : base(context)
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
