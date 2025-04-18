﻿using System.Reflection;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using WarehouseEngine.Api.Configuration;
using WarehouseEngine.Api.Middleware.Auth;
using WarehouseEngine.Application.Implementations;
using WarehouseEngine.Application.Interfaces;
using WarehouseEngine.Domain.Models.Auth;
using WarehouseEngine.Infrastructure.DataContext;

namespace WarehouseEngine.Api;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var services = builder.Services;
        var env = builder.Environment;

        builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
              .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
              .AddJsonFile($"appsettings.local.json", optional: true, reloadOnChange: true);

        var connectionString = builder.Configuration.GetConnectionString("WarehouseEngine")
            ?? throw new ArgumentException("Connection string is not in the app settings");
        services.AddDbContext<IWarehouseEngineContext, WarehouseEngineContext>(options => options.UseSqlServer(connectionString));

        // For Identity
        services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<WarehouseEngineContext>()
            .AddDefaultTokenProviders();

#if DEBUG
        services.AddCors(opt => opt.AddPolicy("localhost", policy => policy.WithOrigins("http://localhost:4201", "https://localhost:4201", "http://127.0.0.1:4201", "https://127.0.0.1:4201").AllowAnyHeader().AllowAnyMethod().WithExposedHeaders("bearer")));
#endif

        // Add services to the container.
        services.Configure<JwtConfiguration>(builder.Configuration.GetSection(nameof(JwtConfiguration)));
        services.ConfigureOptions<ConfigureJwtBearerOptions>();
        services.ConfigureOptions<ConfigureSwaggerGenOptions>();

        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IItemService, ItemService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddTransient<IIdGenerator, SequentialIdGenerator>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer();

        services.AddTransient<IClaimsTransformation, WarehouseClaimsTransformation>();

        // Registers IApiVersionDescriptionProvider for swagger gen and swagger ui
        services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ReportApiVersions = true;
        })
        .AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(option =>
        {
            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            option.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });

            option.ExampleFilters();
        });

        services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());
        services.AddProblemDetails();

        var app = builder.Build();
        app.UseExceptionHandler();

        if (app.Environment.IsDevelopment())
        {
            // Uses the exception handling strategy detailed here:
            // https://learn.microsoft.com/en-us/aspnet/core/fundamentals/error-handling?view=aspnetcore-8.0#produce-a-problemdetails-payload-for-exceptions
            app.UseDeveloperExceptionPage();
        }

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

                foreach (string version in apiVersions)
                {
                    options.SwaggerEndpoint($"/swagger/{version}/swagger.json", version.ToUpperInvariant());
                }
            });
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

#if DEBUG
        app.UseCors("localhost");
#endif

        await app.RunAsync();
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
        IdentityUser? demoUserId = await userManager.FindByNameAsync(demoUser.UserName);
        if (demoUserId is not null) return;

        IdentityResult result = await userManager.CreateAsync(demoUser, "P@ssword1");
        if (!result.Succeeded) throw new ArgumentException(result.ToString());
#endif
    }
}
