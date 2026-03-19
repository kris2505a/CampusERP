using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Domain.Entity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure;
public class TokenService {
    
    public TokenService(IConfiguration config) {
        _config = config;
    }

    public string Generate(User user) {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var key = _config["Jwt:Key"];
        if (key is null) {
            throw new Exception("Jwt Key missing in config");
        }

        var secureKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

        var creds = new SigningCredentials(secureKey, SecurityAlgorithms.HmacSha256);

        int expiry = int.TryParse(_config["Jwt:ExpiryMinutes"], out var e) ? e : 60;

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiry),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private readonly IConfiguration _config;
}
