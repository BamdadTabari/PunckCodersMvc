using DataProvider.EntityFramework.Configs;
using DataProvider.EntityFramework.Services.Identity;
using DataProvider.EntityFramework.Services.Weblog;
using Serilog;

namespace DataProvider.EntityFramework.Repository;
public interface IUnitOfWork : IDisposable
{
    IPostRepo PostRepo { get; }
    IPostCommentRepo PostCommentRepo { get; }

    IUserRepo UserRepo { get; }
    IRoleRepo RoleRepo { get; }
    IUserRoleRepo UserRoleRepo { get; }
    ITokenBlacklistRepository TokenBlacklistRepo { get; }

    IEmailRepo EmailRepo { get; }

    Task<bool> CommitAsync();
}

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private readonly ILogger _logger;
    #region Blog
    public IPostRepo PostRepo { get; }
    public IPostCommentRepo PostCommentRepo { get; }
    #endregion

    #region Identity
    public IUserRepo UserRepo { get; }
    public IRoleRepo RoleRepo { get; }
    public IUserRoleRepo UserRoleRepo { get; }
    public ITokenBlacklistRepository TokenBlacklistRepo { get; }
    #endregion

    #region Email
    public IEmailRepo EmailRepo { get; }
    #endregion

    public async Task<bool> CommitAsync() => await _context.SaveChangesAsync() > 0;

    // dispose and add to garbage collector
    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        _logger = new LoggerConfiguration().WriteTo.Console()  // Also log to the console
        .CreateLogger();

        #region Blog
        PostRepo = new PostRepo(_context, _logger);
        PostCommentRepo = new PostCommentRepo(_context, _logger);
        #endregion

        #region Identity
        UserRepo = new UserRepo(_context, _logger);
        RoleRepo = new RoleRepo(_context, _logger);
        UserRoleRepo = new UserRoleRepo(_context, _logger);
        TokenBlacklistRepo = new TokenBlacklistRepo(_context, _logger);
        #endregion

        #region Email
        EmailRepo = new EmailRepo();
        #endregion
    }
}
