using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AppHost.ApiService.Entities.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AppHost.ApiService.Services.Identity;

public sealed class JwtTokenService : IJwtTokenService
{
    private readonly JwtOptions _options;
    private readonly SigningCredentials _signingCredentials;

    public JwtTokenService(IOptions<JwtOptions> options)
    {
        _options = options.Value;

        if (string.IsNullOrWhiteSpace(_options.Issuer) ||
            string.IsNullOrWhiteSpace(_options.Audience) ||
            _options.ExpiresMinutes <= 0)
        {
            throw new InvalidOperationException("JWT configuration is invalid.");
        }

        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_options.Key));

        _signingCredentials = new SigningCredentials(
            securityKey,
            SecurityAlgorithms.HmacSha256);
    }

    public string CreateToken(User user)
    {
        var now = DateTime.UtcNow;
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Name, user.Name),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("role", "User"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            notBefore: now,
            expires: now.AddMinutes(_options.ExpiresMinutes),
            signingCredentials: _signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
