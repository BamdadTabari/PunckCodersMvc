using DataProvider.EntityFramework.Configs;
using DataProvider.EntityFramework.Entities.Identity;
using DataProvider.EntityFramework.Repository;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace DataProvider.EntityFramework.Services.Identity;
public interface IUserRepo : IRepository<User>
{
    Task<User?> GetUser(string usernameOrEmail);
    Task<User?> GetUser(int Id);
    Task<bool> AnyExistUserName(string username);
    Task<bool> AnyExistEmail(string email);
}
public class UserRepo : Repository<User>, IUserRepo
{
    private readonly IQueryable<User> _queryable;

    private readonly ILogger _logger;

    public UserRepo(AppDbContext context, ILogger logger) : base(context)
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
