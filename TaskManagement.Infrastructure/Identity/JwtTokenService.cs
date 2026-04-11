using Microsoft.Extensions.Configuration;//for IConfiguration and read from appsetting.json (key,issuer, audience, expire time)
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManagement.Application.Interfaces;

namespace TaskManagement.Infrastructure.Identity;

public class JwtTokenService : IJwtTokenService
{
    private readonly IConfiguration _configuration;

    public JwtTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(string userId, string email, IList<string> roles)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Email, email)
        };

        // اضافه کردن role ها
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]!)//secret key to byte array for signing the token
        );

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);//create signing credentials using the key and specify the algorithm (HMAC SHA256)

        var expireDate = DateTime.UtcNow.AddMinutes(
            Convert.ToDouble(_configuration["JwtSettings:ExpireMinutes"])
        );

        var token = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:Issuer"],
            audience: _configuration["JwtSettings:Audience"],
            claims: claims,
            expires: expireDate,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);//generate the token and return it as a string
    }

    public DateTime GetTokenExpiration()
    {
        return DateTime.UtcNow.AddMinutes(
            Convert.ToDouble(_configuration["JwtSettings:ExpireMinutes"])
        );
    }
}