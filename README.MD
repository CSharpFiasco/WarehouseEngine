
# Warehouse Engine

This is not a production ready application. This is an example of a Warehouse Engine built using .NET 8, EF Core, and Clean Architecture principles.

## Lightweight CQRS migration

Warehouse is the first migrated feature. Its existing authenticated GET routes
(`warehouse/{id}`, `warehouse/list`, and `warehouse/count`) use `IWarehouseQueries`
with database-side DTO projections. POST `warehouse` accepts a
`CreateWarehouseCommand`, creates the domain entity through `Warehouse.Create`,
and saves directly through the existing EF-backed context. No mediator,
repository wrapper, or additional unit-of-work implementation is introduced.

The POST JSON contract remains `{ "name": "Main Warehouse" }`; supplied IDs are
ignored and IDs are generated server-side. Success remains an empty 201 response,
and a duplicate generated ID returns 409. Required names and the existing
32-character limit are enforced before saving; invalid names return a 400
validation problem. The domain factory also enforces those rules for non-HTTP
callers. Domain entities are not request models. Warehouse queries and saving
propagate request cancellation without changing the schema or database keys.

Other features still use their existing services and will migrate incrementally.
The context defaults to no tracking, so future commands that modify existing
aggregates must explicitly use `AsTracking()` and load the state their behavior
requires. Keep shared use-case coordination when there are multiple callers;
add persistence contracts or execution pipelines only when they carry useful
policy.

## Guarded domain entities

Entities are created through `Create` factories, not public constructors or
object initializers. Scalar properties and individual navigation references have
private setters; named domain methods validate complete proposed changes before
assigning any fields. The `*.Behavior.cs` partials keep this behavior separate
from EF mapping annotations. Factories enforce non-empty identities, required
nonblank text, existing field-length limits, and valid address contents.
Optional fields remain optional.

Warehouse stock and prices must be nonnegative; order, out-of-stock, and
purchase-order line quantities must be positive. Order-status byte values and
transitions remain unchanged until their business policy is defined.
DTO conversions and test seeds use the factories, and existing services invoke
domain methods on explicitly tracked entities. HTTP input validation uses the
same text limits and rejects invalid input with a 400 validation problem.
Successful HTTP contracts and the database schema are unchanged.

As with Warehouse, collection navigations remain EF-managed mutable collections;
this is not yet a redesign of aggregate ownership. EF's private construction path
is retained for materialization and does not run factory validation. Validating
legacy stored data and protecting relationship-level invariants require a
separate persistence/aggregate policy.

## Run with Aspire

Prerequisites:

- .NET 10 SDK
- Aspire CLI 13.4 or later
- Docker Desktop running with Linux containers
- Node.js and npm

From the repository root, run:

```powershell
aspire run --apphost src/WarehouseEngine.AppHost/WarehouseEngine.AppHost.csproj
```

The AppHost provisions SQL Server with a persistent data volume, creates the `WarehouseEngine` database, applies EF Core migrations, starts the API after migration succeeds, and then runs the Angular application at `http://localhost:4201`.

Aspire injects the `WarehouseEngine` connection string into both the migration service and API. An `appsettings.local.json` connection string is not required when running through Aspire.

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

CLI tests use native Microsoft.Testing.Platform (MTP), selected in `global.json`,
with the xUnit MTP runner enabled in each test project. The SDK selection stays
on stable .NET 10 feature bands, even when newer preview SDKs are installed.

From the repository root:

```powershell
dotnet test
dotnet test --project tests\WarehouseEngine.Domain.Tests\WarehouseEngine.Domain.Tests.csproj
dotnet test --project tests\WarehouseEngine.Application.Tests\WarehouseEngine.Application.Tests.csproj --filter-class WarehouseEngine.Application.Tests.Implementations.WarehouseQueriesTests
dotnet test --project tests\WarehouseEngine.Domain.Tests\WarehouseEngine.Domain.Tests.csproj --coverlet --coverlet-output-format cobertura
```

Native MTP uses `--project` instead of positional project paths, xUnit filters
such as `--filter-class` instead of VSTest `--filter`, and `--coverlet` instead
of `--collect:"XPlat Code Coverage"`. Do not add an extra `--` before test options.
The Visual Studio adapter remains available for Test Explorer. Application tests
require LocalDB on Windows; API integration tests require Docker.

### WarehouseEngine.Api.Integration.Tests

Integration tests against a real instance of the API using TestContainers for spinning up dependent services.

### Warehouse.Domain.Tests

Unit tests against domain factories, guarded changes, and value objects. JSON
request/response serialization belongs in DTO and API tests, not domain entity
construction.

### Warehouse.Application.Tests

Integration tests against the application and infrastructure layers using TestContainers for spinning up a SQL Server database. This tests business logic and data access logic such as querying data and turning them to DTOs.