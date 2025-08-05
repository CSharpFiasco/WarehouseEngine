using DotNet.Testcontainers.Builders;
using Microsoft.EntityFrameworkCore;
using Testcontainers.MsSql;
using WarehouseEngine.Infrastructure.DataContext;
using Xunit;

namespace WarehouseEngine.Api.Integration.Tests.Factories;

public class TestDatabaseManager : IAsyncLifetime
{
    private readonly MsSqlContainer _msSqlContainer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2025-CTP2.1-ubuntu-22.04")
        .WithEnvironment("ACCEPT_EULA", "Y")
        .WithWaitStrategy(Wait.ForUnixContainer().UntilCommandIsCompleted("/opt/mssql-tools18/bin/sqlcmd", "-C", "-Q", "SELECT 1;"))
        .WithPortBinding(54965, 1433)
        .Build();

    private static readonly SemaphoreSlim _lock = new(1, 1);
    private static bool _databaseInitialized = false;

    public string ConnectionString => _msSqlContainer.GetConnectionString() + ";Initial Catalog=WarehouseEngine";

    private readonly Action<WarehouseEngineContext> _seedDatabase;
    public TestDatabaseManager(Action<WarehouseEngineContext> seedDatabase)
    {
        _seedDatabase = seedDatabase;
    }

    public async ValueTask InitializeAsync()
    {
        await _msSqlContainer.StartAsync(TestContext.Current.CancellationToken);

        await _lock.WaitAsync(TimeSpan.FromMicroseconds(1_000), TestContext.Current.CancellationToken);
        try
        {
            if (!_databaseInitialized)
            {
                using var context = CreateContext();
                await context.Database.EnsureDeletedAsync(TestContext.Current.CancellationToken);
                await context.Database.EnsureCreatedAsync(TestContext.Current.CancellationToken);
                _seedDatabase(context);
                await context.SaveChangesAsync(TestContext.Current.CancellationToken);
                _databaseInitialized = true;
            }
        }
        finally
        {
            _lock.Release();
        }
    }

    public WarehouseEngineContext CreateContext()
    {
        var builder = new DbContextOptionsBuilder<WarehouseEngineContext>();
        builder.UseSqlServer(ConnectionString);
        return new WarehouseEngineContext(builder.Options);
    }

    public async ValueTask DisposeAsync()
    {
        await _msSqlContainer.DisposeAsync();
    }
}
