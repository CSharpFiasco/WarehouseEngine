using Microsoft.EntityFrameworkCore;
using WarehouseEngine.Application.Implementations;
using WarehouseEngine.Application.Interfaces;
using WarehouseEngine.Infrastructure.Data;

namespace WarehouseEngine.WebApi;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var env = builder.Environment;

        builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
              .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        if (connectionString is null) throw new ArgumentException("Invalid Connection String");

        builder.Services.AddDbContext<IWarehouseEngineContext, WarehouseEngineContext>(options =>
                options.UseSqlServer(connectionString));

        builder.Services.AddScoped<IItemService, ItemService>();

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
