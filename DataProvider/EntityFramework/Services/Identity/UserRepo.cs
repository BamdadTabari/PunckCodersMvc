using DataProvider.Assistant.Pagination;
using DataProvider.EntityFramework.Configs;
using DataProvider.EntityFramework.Entities.Identity;
using DataProvider.EntityFramework.Extensions.Identity;
using DataProvider.EntityFramework.Repository;
using DataProvider.Models.Query.Identity.User;
using Microsoft.EntityFrameworkCore;

namespace DataProvider.EntityFramework.Services.Identity;
public interface IUserRepo : IRepository<User>
{
    Task<User?> GetUser(string usernameOrEmail);
    Task<User?> GetUser(int Id);
    Task<bool> AnyExistUserName(string username);
    Task<bool> AnyExistEmail(string email);
    PaginatedList<User> GetPaginated(GetPagedUserQuery filter);
}
public class UserRepo : Repository<User>, IUserRepo
{
    private readonly IQueryable<User> _queryable;

    private readonly Serilog.ILogger _logger;

    public UserRepo(AppDbContext context, Serilog.ILogger logger) : base(context)
    {
        _queryable = DbContext.Set<User>();
        _logger = logger;
    }

    /// <summary>
    /// Check if user exist, in case not exist return false, in case query error return false, in case exist return true
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public async Task<bool> AnyExistEmail(string email)
    {
        try
        {
            return await _queryable.AnyAsync(x => x.Email == email);
        }
        catch (Exception ex)
        {
            _logger.Error("Error in User AnyAsync", ex);
            return await Task.FromResult(false);
        }
    }

    /// <summary>
    /// Check if user exist, in case not exist return false, in case query error return false, in case exist return true
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    public async Task<bool> AnyExistUserName(string username)
    {
        try
        {
            return await _queryable.AnyAsync(x => x.Username == username);
        }
        catch (Exception ex)
        {
            _logger.Error("Error in User AnyExistUserName", ex);
            return await Task.FromResult(false);
        }
    }

    public PaginatedList<User> GetPaginated(GetPagedUserQuery filter)
    {
        try
        {
            var query = _queryable.Where(x => x.Id > filter.LastId).AsNoTracking().ApplyFilter(filter).ApplySort(filter.SortBy);
            var dataTotalCount = _queryable.Count();
            return new PaginatedList<User>([.. query], dataTotalCount, filter.Page, filter.PageSize);
        }
        catch
        {
            _logger.Error("Error in GetPaginatedUser");
            return new PaginatedList<User>([], 0, filter.Page, filter.PageSize);
        }
    }

    /// <summary>
    /// Get User by username Or Email, in case not exist return new User, in query error return new User
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    public async Task<User?> GetUser(string usernameOrEmail)
    {
        try
        {
            return await _queryable.FirstOrDefaultAsync(x => x.Username == usernameOrEmail || x.Email == usernameOrEmail);
        }
        catch (Exception ex)
        {
            _logger.Error("Error in Get User", ex);
            return null;
        }
    }

    public async Task<User?> GetUser(int Id)
    {
        try
        {
            return await _queryable.SingleOrDefaultAsync(x => x.Id == Id);
        }
        catch (Exception ex)
        {
            _logger.Error("Error in Get User By Id", ex);
            return null;
        }
    }
}
