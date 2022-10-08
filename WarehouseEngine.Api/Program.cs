using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using WarehouseEngine.Api.Configuration;
using WarehouseEngine.Application.Implementations;
using WarehouseEngine.Application.Interfaces;
using WarehouseEngine.Domain.Models.Login;
using WarehouseEngine.Infrastructure.Data;

namespace WarehouseEngine.Api;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        });

        var connectionString = builder.Configuration.GetConnectionString("WarehouseEngine");
        if (connectionString is null) throw new ArgumentException("Connection string is not in the app settings");

        builder.Services.AddDbContext<IWarehouseEngineContext, WarehouseEngineContext>(options => options.UseSqlServer(connectionString));

        // For Identity
        builder.Services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<WarehouseEngineContext>()
            .AddDefaultTokenProviders();

        // Add services to the container.
        builder.Services.Configure<JwtConfiguration>(builder.Configuration.GetSection(nameof(JwtConfiguration)));
        builder.Services.ConfigureOptions<ConfigureJwtBearerOptions>();
        builder.Services.ConfigureOptions<ConfigureSwaggerGenOptions>();

        builder.Services.AddScoped<IJwtService, JwtService>();
        builder.Services.AddScoped<IItemService, ItemService>();

        // Registers IApiVersionDescriptionProvider for swagger gen and swagger ui
        builder.Services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
            options.ReportApiVersions = true;
        });

        builder.Services.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();
        await SeedData(app.Services);

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                using var scope = app.Services.CreateScope();
                var provider = scope.ServiceProvider.GetRequiredService<IApiVersionDescriptionProvider>();
                var apiVersions = provider.ApiVersionDescriptions.Select(d => d.GroupName);

                foreach (var version in apiVersions)
                {
                    options.SwaggerEndpoint($"/swagger/{version}/swagger.json", version.ToUpperInvariant());
                }
            });
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }

    public static async Task SeedData(IServiceProvider services)
    {
#if DEBUG

        using var scope = services.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

        var demoUser = new IdentityUser
        {
            Email = "demo@carlosmartos.com",
            UserName = "demo"
        };
        var demoUserId = await userManager.FindByNameAsync(demoUser.UserName);
        if (demoUserId is not null) return;

        var result = await userManager.CreateAsync(demoUser, "P@ssword1");
        if (!result.Succeeded) throw new ArgumentException(result.ToString());
#endif
    }
}
