using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WarehouseEngine.Domain.Models.Auth;

namespace WarehouseEngine.Api.Configuration;
public class ConfigureJwtBearerOptions : IConfigureNamedOptions<JwtBearerOptions>
{
    private readonly JwtConfiguration _configuration;
    public ConfigureJwtBearerOptions(IOptions<JwtConfiguration> configurations)
    {
        _configuration = configurations.Value;
    }

    public void Configure(string? name, JwtBearerOptions options)
    {
        Configure(options);
    }
    public void Configure(JwtBearerOptions options)
    {
        static bool Validator(DateTime? before, DateTime? expired, SecurityToken token, TokenValidationParameters parameters)
        {
            return expired != null && DateTime.UtcNow < expired;
        }

        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Secret));

        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = _configuration.ValidIssuer,

            ValidateAudience = true,
            ValidAudience = _configuration.ValidAudience,

            ValidateLifetime = true,

            RequireExpirationTime = true,

            IssuerSigningKey = authSigningKey,
            LifetimeValidator = Validator
        };
    }
}
