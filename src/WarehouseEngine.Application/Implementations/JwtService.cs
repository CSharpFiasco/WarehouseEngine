using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WarehouseEngine.Application.Interfaces;
using WarehouseEngine.Domain.Models.Auth;

namespace WarehouseEngine.Application.Implementations;
public class JwtService : IJwtService
{
    private readonly JwtConfiguration _configuration;
    public JwtService(IOptions<JwtConfiguration> configuration)
    {
        _configuration = configuration.Value;
    }

    public string GetNewToken(List<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Secret));

        var token = new JwtSecurityToken(
            issuer: _configuration.ValidIssuer,
            audience: _configuration.ValidAudience,
            expires: DateTime.Now.AddHours(10), // We could shorten this with the help of refresh tokens
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
