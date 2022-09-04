using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WarehouseEngine.Domain.Models.Login;

namespace WarehouseEngine.Api.Configuration;
public class ConfigureJwtBearerOptions : IConfigureNamedOptions<JwtBearerOptions>
{
    private readonly IOptions<JwtConfiguration> _configuration;
    public ConfigureJwtBearerOptions(IOptions<JwtConfiguration> configurations)
    {
        _configuration = configurations;
    }

    public void Configure(JwtBearerOptions options)
    {
        static bool Validator(DateTime? before, DateTime? expired, SecurityToken token, TokenValidationParameters parameters)
        {
            return expired != null && DateTime.UtcNow < expired;
        }

        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Value.Secret));

        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = "https://example.com",

            ValidateAudience = true,
            ValidAudience = "https://warehouseengine.example.com",

            ValidateLifetime = true,

            RequireExpirationTime = true,

            IssuerSigningKey = authSigningKey,
            LifetimeValidator = Validator
        };
    }

    public void Configure(string? name, JwtBearerOptions options)
    {
        Configure(options);
    }
}
