using DataProvider.EntityFramework.Configs;
using DataProvider.EntityFramework.Entities.Identity;
using DataProvider.EntityFramework.Repository;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProvider.EntityFramework.Services.Identity;
public interface ITokenBlacklistRepository : IRepository<BlacklistedToken>
{
    Task<bool> IsTokenBlacklisted(string token); // Check if a token is blacklisted
    Task<List<BlacklistedToken>?> GetExpiredTokensAsync(); // Optional: Clean up expired tokens
}

public class TokenBlacklistRepo : Repository<BlacklistedToken>, ITokenBlacklistRepository
{
    private readonly IQueryable<BlacklistedToken> _queryable;

    private readonly ILogger _logger;

    public TokenBlacklistRepo(AppDbContext context, ILogger logger) : base(context)
    {
        _queryable = DbContext.Set<BlacklistedToken>();
        _logger = logger;
    }

    public async Task<bool> IsTokenBlacklisted(string token)
    {
        try
        {
            return await _queryable.AnyAsync(x => x.Token == token && x.ExpiryDate > DateTime.UtcNow);
        }
        catch (Exception ex)
        {
            _logger.Error("Error in IsTokenBlacklisted", ex);
            return false;
        }
    }

    public async Task<List<BlacklistedToken>?> GetExpiredTokensAsync()
    {
        
        try
        {
            return await _queryable.Where(t => t.ExpiryDate <= DateTime.UtcNow).ToListAsync();
            
        }
        catch (Exception ex)
        {
            _logger.Error("Error in GetExpiredTokens", ex);
            return null;
        }
    }
}