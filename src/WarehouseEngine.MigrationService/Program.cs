using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WarehouseEngine.Infrastructure.DataContext;

var builder = Host.CreateApplicationBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("WarehouseEngine")
    ?? throw new InvalidOperationException("Connection string 'WarehouseEngine' was not found.");

builder.Services.AddDbContext<WarehouseEngineContext>(options => options.UseSqlServer(connectionString));

using var host = builder.Build();
using var scope = host.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<WarehouseEngineContext>();

await context.Database.MigrateAsync();