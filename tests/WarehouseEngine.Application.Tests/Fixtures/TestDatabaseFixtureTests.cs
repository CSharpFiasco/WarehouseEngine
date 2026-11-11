using System.Runtime.InteropServices;
using WarehouseEngine.Infrastructure.DataContext;

namespace WarehouseEngine.Application.Tests.Fixtures;

[ExcludeFromCodeCoverage]
public class TestDatabaseFixtureTests
{
    [Fact]
    public void CreateOptions_ConfiguredConnection_UsesSqlServer()
    {
        const string connectionString = "Server=localhost,1433;Database=WarehouseEngineTest;Integrated Security=true;TrustServerCertificate=true;";

        using var context = new WarehouseEngineContext(TestDatabaseFixture.CreateOptions(connectionString));

        Assert.Equal("Microsoft.EntityFrameworkCore.SqlServer", context.Database.ProviderName);
        Assert.Equal("localhost,1433", context.Database.GetDbConnection().DataSource);
        Assert.Equal("WarehouseEngineTest", context.Database.GetDbConnection().Database);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void CreateOptions_MissingConnection_UsesSupportedLocalDefaultOrThrows(string? connectionString)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            using var context = new WarehouseEngineContext(TestDatabaseFixture.CreateOptions(connectionString));

            Assert.Equal("Microsoft.EntityFrameworkCore.SqlServer", context.Database.ProviderName);
            Assert.Contains(@"Server=(localdb)\mssqllocaldb;", context.Database.GetConnectionString());
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            using var context = new WarehouseEngineContext(TestDatabaseFixture.CreateOptions(connectionString));

            Assert.Equal("Microsoft.EntityFrameworkCore.Sqlite", context.Database.ProviderName);
            Assert.Equal("Data Source=.\\warehousetest.db", context.Database.GetConnectionString());
        }
        else
        {
            var exception = Assert.Throws<InvalidOperationException>(() => TestDatabaseFixture.CreateOptions(connectionString));

            Assert.Contains("ConnectionStrings__DefaultConnection", exception.Message);
        }
    }
}
