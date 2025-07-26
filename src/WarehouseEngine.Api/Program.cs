using System.Text.Json.Serialization;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using WarehouseEngine.Api.Configuration;
using WarehouseEngine.Api.Examples;
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

        if (env.EnvironmentName != "Integration")
        {
            builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                  .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                  .AddJsonFile($"appsettings.local.json", optional: true, reloadOnChange: true);
        }

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

        // Using ..Configure<JsonOptions> rather than .AddJsonOptions() according to docs
        //https://learn.microsoft.com/en-us/aspnet/core/fundamentals/openapi/include-metadata?view=aspnetcore-10.0&tabs=minimal-apis#mvc-json-options-and-global-json-options
        services.Configure<JsonOptions>(options =>
        {
            options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            options.SerializerOptions.MaxDepth = 128;
        });

        services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });
        services.AddOpenApi(options =>
        {
            options.AddOperationTransformer((operation, context, cancellationToken) =>
            {
                if (operation.Tags.Select(t => t.Name).Contains("Authenticate"))
                {
                    var authenticate200Response = operation.Responses.FirstOrDefault(x => x.Key == "200").Value;
                    if (authenticate200Response != null)
                    {
                        authenticate200Response.Headers.Add("Bearer", new OpenApiHeader() { Description = "Contains JWT" });
                    }
                }

                return Task.CompletedTask;
            });

            options.AddSchemaTransformer((schema, context, cancellationToken) =>
            {
                var type = context.JsonTypeInfo.Type;
                if (ExampleDictionary.Examples.TryGetValue(type, out var exampleValue))
                {
                    schema.Example = exampleValue;
                }

                return Task.CompletedTask;
            });
        });
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
            // configure openapi here
            app.MapOpenApi();
            app.MapScalarApiReference();
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
