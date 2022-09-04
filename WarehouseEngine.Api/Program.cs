using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WarehouseEngine.Api.Configuration;
using WarehouseEngine.Application.Implementations;
using WarehouseEngine.Application.Interfaces;
using WarehouseEngine.Domain.Models.Login;
using WarehouseEngine.Infrastructure.Data;
using WarehouseEngine.Infrastructure.Implementations;

namespace WarehouseEngine.Api;

public static class Program
{
    public static void Main(string[] args)
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

        builder.Services.AddScoped<IJwtService, JwtService>();
        builder.Services.AddScoped<IItemService, ItemService>();

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
