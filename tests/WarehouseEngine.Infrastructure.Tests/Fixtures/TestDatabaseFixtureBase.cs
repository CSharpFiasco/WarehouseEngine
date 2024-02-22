﻿using Microsoft.EntityFrameworkCore;
using WarehouseEngine.Infrastructure.DataContext;

namespace WarehouseEngine.Infrastructure.Tests.Fixtures;

public sealed class TestDatabaseFixture
{
    /*
     * TODO: Pull from environment variables against an instance of SQL Server 2022
     * VS 2022 uses SQL Serveer 2019
     */
    private readonly string ConnectionString = @$"Server=(localdb)\mssqllocaldb;Database=WarehouseTestDatabase;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=False;Application Name=""Warehouse Engine Integration Tests""";

    private static readonly object _lock = new();
    private static bool _databaseInitialized = false;

    public TestDatabaseFixture()
    {
        lock (_lock)
        {
            if (!_databaseInitialized)
            {
                using WarehouseEngineContext context = CreateContext();

                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                SeedDatabase(context);
                context.SaveChanges();

                _databaseInitialized = true;
            }
        }
    }

    private static void SeedDatabase(WarehouseEngineContext context)
    {

    }

    public WarehouseEngineContext CreateContext()
        => new WarehouseEngineContext(
            new DbContextOptionsBuilder<WarehouseEngineContext>()
                .UseSqlServer(ConnectionString)
                .Options);

}
