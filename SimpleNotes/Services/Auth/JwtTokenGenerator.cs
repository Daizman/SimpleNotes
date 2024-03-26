using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SimpleNotes.Abstract;
using SimpleNotes.Models.User;

namespace SimpleNotes.Services.Auth;

public class JwtTokenGenerator(
    IDateTimeProvider dtProvider,
    IOptions<Settings.JwtTokenGenerator> settings,
    IMemoryCache memoryCache) : IJwtTokenGenerator
{
    private readonly Settings.JwtTokenGenerator _settings = settings.Value;
    
    public string GenerateToken(User user)
    {
        SigningCredentials signingCredentials = new(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Secret)),
            SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.GivenName, user.NickName),
            new Claim(JwtRegisteredClaimNames.FamilyName, user.NickName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };
        var expiration = dtProvider.UtcNow.AddMinutes(_settings.ExpiryMinutes);

        JwtSecurityToken securityToken = new(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            expires: expiration,
            claims: claims,
            signingCredentials: signingCredentials
        );

        var token = new JwtSecurityTokenHandler().WriteToken(securityToken);

        memoryCache.Set(user.Id, token, expiration);

        return token;
    }
}