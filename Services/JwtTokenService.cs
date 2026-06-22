using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SecureFintechApi.Models;
using SecureFintechApi.Settings;

namespace SecureFintechApi.Services;

public class JwtTokenService
{
    private readonly JwtSettings _jwtSettings;

    public JwtTokenService(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }

    public string GenerateToken(User user)
    {
        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        SymmetricSecurityKey securityKey = new(
            Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)
        );

        SigningCredentials credentials = new(
            securityKey,
            SecurityAlgorithms.HmacSha256
        );

        JwtSecurityToken token = new(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public DateTime GetExpirationDate()
    {
        return DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes);
    }
}
