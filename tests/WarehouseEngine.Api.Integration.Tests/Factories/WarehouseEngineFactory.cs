using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using WarehouseEngine.Infrastructure.DataContext;

namespace WarehouseEngine.Api.Integration.Tests.Factories;
public class WarehouseEngineFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // set EnvironmentName as integration rather than development


        builder.ConfigureAppConfiguration((context, config) =>
        {

            builder.ConfigureServices(services =>
            {
                // Remove the existing WarehouseEngineContext registration
                var dbContextDescriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<WarehouseEngineContext>));

                ArgumentNullException.ThrowIfNull(dbContextDescriptor);

                services.Remove(dbContextDescriptor);
            });
        });

        builder.UseEnvironment("Integration");
    }
}
