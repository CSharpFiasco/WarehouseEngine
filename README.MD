
# Warehouse Engine

This is not a production ready application. This is an example of a Warehouse Engine built using .NET 8, EF Core, and Clean Architecture principles.

## Database Deployment Strategy

This application uses Entity Framework Core (EF Core) for database management. The database is created and updated using EF Core migrations. The startup project and the data project are the same project in this case.

You can get started by adding an appsettings.local.json file to the WarehouseEngine.Infrastructure project with the following content with your connection string:

```
{
  "ConnectionStrings": {
    "WarehouseEngine": "Server=(localdb)\\MSSQLLocalDB;Database=WarehouseEngine;Trusted_Connection=True"
  }
}
```

## Testing Strategy

### WarehouseEngine.Api.Integration.Tests

Integration tests against a real instance of the API using TestContainers for spinning up dependent services.

### Warehouse.Domain.Tests

Unit tests against the domain layer including entities, value objects, and domain services. Focuses on making sure they serialize correctly. TODO: this may not be needed anymore since we have moved to using DTOs.

### Warehouse.Application.Tests

Integration tests against the application and infrastructure layers using TestContainers for spinning up a SQL Server database. This tests business logic and data access logic such as querying data and turning them to DTOs.