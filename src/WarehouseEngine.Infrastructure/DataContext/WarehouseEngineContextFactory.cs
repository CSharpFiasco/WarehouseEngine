using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace WarehouseEngine.Infrastructure.DataContext;

public class WarehouseEngineContextFactory : IDesignTimeDbContextFactory<WarehouseEngineContext>
{
    public WarehouseEngineContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.local.json", true)
                .Build();


        var dataConnectionString = configuration.GetConnectionString("WarehouseEngine")
            ?? throw new ArgumentException("Connection string is not in the app settings");

        var optionsBuilder = new DbContextOptionsBuilder<WarehouseEngineContext>();

        optionsBuilder.UseSqlServer(dataConnectionString);

        return new WarehouseEngineContext(optionsBuilder.Options);
    }
}
