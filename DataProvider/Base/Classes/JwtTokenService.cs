using DataProvider.Certain.Configs;
using DataProvider.EntityFramework.Entities.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class JwtTokenService
{
    private readonly SecurityTokenConfig _configuration;

    public JwtTokenService(SecurityTokenConfig configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(User user, IEnumerable<string> roles)
    {

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new (JwtRegisteredClaimNames.Sub, user.Username),
            new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new ("userId", user.Id.ToString())
        };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var token = new JwtSecurityToken(
            issuer: _configuration.Issuer,
            audience: _configuration.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_configuration.AccessTokenLifetime.TotalMinutes),
            signingCredentials: creds

        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GetUserIdFromClaims(ClaimsPrincipal user)
    {
        // Attempt to find the 'userId' claim
        var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == "userId");
        return userIdClaim?.Value; // Returns the userId as a string, or null if not found
    }

    public int GetTokenExpiryMinutes(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

        if (jwtToken == null)
            throw new ArgumentException("Invalid token");

        var expClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp);

        if (expClaim == null)
            throw new ArgumentException("Token does not contain an expiration claim");

        var expDateTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expClaim.Value)).UtcDateTime;
        var remainingMinutes = (int)(expDateTime - DateTime.UtcNow).TotalMinutes;

        return remainingMinutes > 0 ? remainingMinutes : 0; // Ensure no negative values
    }
}
