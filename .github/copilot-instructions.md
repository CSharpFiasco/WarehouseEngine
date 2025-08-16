# Copilot Instructions for WarehouseEngine

## Repository Overview

**WarehouseEngine** is a full-stack warehouse management system built with .NET 9 and Angular 18. It follows a database-first approach using Entity Framework Core and implements Clean Architecture principles with Domain, Application, Infrastructure, and API layers.

### High-Level Architecture
- **Backend**: ASP.NET Core Web API (.NET 9) with JWT authentication, Entity Framework Core, SQL Server
- **Frontend**: Angular 18 with NgRx state management, Angular Material UI, TypeScript/SCSS
- **Database**: SQL Server with Entity Framework Core (database-first approach)
- **Testing**: xUnit for .NET, Jasmine/Karma for Angular, integration tests with Testcontainers
- **Target Runtime**: .NET 9, Angular 18

## Build and Development Instructions

### Prerequisites and Environment Setup
1. **ALWAYS ensure SQL Server is available** - The application requires a SQL Server instance
2. **ALWAYS create `appsettings.local.json`** in `src/WarehouseEngine.Api/` with database connection:
   ```json
   {
     "ConnectionStrings": {
       "WarehouseEngine": "Server=(localdb)\\MSSQLLocalDB;Database=WarehouseEngine;Trusted_Connection=True"
     }
   }
   ```
3. **ALWAYS run these certificate commands first** (documented workaround):
   ```
   dotnet dev-certs https --clean
   dotnet dev-certs https --trust
   dotnet dev-certs https --check
   ```

### Build Process (VALIDATED - Takes ~12-24 seconds)
```bash
# Build entire solution (ALWAYS run from repository root)
dotnet build

# Clean build if needed
dotnet clean
dotnet build
```

### Testing (VALIDATED - Takes ~20+ seconds for full test suite)
```bash
# Run all tests (includes 27 integration tests using Testcontainers)
dotnet test

# Backend tests only
dotnet test tests/

# Frontend tests (from src/WarehouseEngine.UI directory)
npm test
```

### Development Servers
```bash
# Backend API (runs on https://localhost:7088, http://localhost:5220)
dotnet run --project src/WarehouseEngine.Api/WarehouseEngine.Api.csproj

# Frontend (runs on http://localhost:4201)
cd src/WarehouseEngine.UI
npm start
```

### Frontend Build and Lint (VALIDATED)
```bash
cd src/WarehouseEngine.UI

# Install dependencies (ALWAYS run before other npm commands)
npm install

# Build frontend (~8 seconds)
npm run build

# Lint frontend
npm run lint

# Test frontend (~4-5 seconds, 11 tests)
npm test
```

## Project Structure and Key Files

### Solution Organization
```
WarehouseEngine.sln (root solution file)
??? src/
?   ??? WarehouseEngine.Api/           # ASP.NET Core Web API
?   ??? WarehouseEngine.Application/   # Application layer (services, DTOs)
?   ??? WarehouseEngine.Domain/        # Domain entities and models
?   ??? WarehouseEngine.Infrastructure/ # EF Core context, data access
?   ??? WarehouseEngine.UI/            # Angular frontend
?   ??? WarehouseEngine.Database/      # SQL Server database project
??? tests/
    ??? WarehouseEngine.Api.Integration.Tests/  # Integration tests
    ??? WarehouseEngine.Domain.Tests/
    ??? WarehouseEngine.Infrastructure.Tests/
```

### Critical Configuration Files
- `src/WarehouseEngine.Api/appsettings.Development.json` - Development configuration
- `src/WarehouseEngine.Api/Properties/launchSettings.json` - Launch profiles
- `src/WarehouseEngine.UI/angular.json` - Angular CLI configuration
- `src/WarehouseEngine.UI/package.json` - NPM dependencies and scripts
- `src/WarehouseEngine.UI/.eslintrc.json` - ESLint configuration
- `.editorconfig` - Code formatting rules

### Database Context and Entity Framework
- **DbContext**: `WarehouseEngine.Infrastructure.DataContext.WarehouseEngineContext`
- **Entities**: Company, Customer, Employee, Item, Order, Vendor, Warehouse, etc.
- **Connection String**: Must be in appsettings.local.json (never commit this file)
- **Database Provider**: SQL Server with EF Core 9.0.3

### Key Ports and URLs
- **API**: https://localhost:7088 (HTTPS), http://localhost:5220 (HTTP)
- **Frontend**: http://localhost:4201
- **OpenAPI/Swagger**: Available at `/scalar/v1` in development
- **Integration Tests**: Use Testcontainers on port 54965

## Testing Strategy and Validation

### Unit/Integration Tests (.NET)
- **Framework**: xUnit v3
- **Database**: Uses Testcontainers with SQL Server 2025-CTP2.1
- **Integration Tests**: 27 tests covering API endpoints with JWT authentication
- **Database Fixtures**: Automatic seeding with test data
- **Time**: ~20+ seconds for full test suite

### Frontend Tests (Angular)
- **Framework**: Jasmine with Web Test Runner
- **Files**: 9 test files, 11 total tests
- **Time**: ~4-5 seconds
- **Coverage**: Components, services, guards

### Known Issues and Workarounds
1. **Certificate Issue**: Always run the `dotnet dev-certs` commands before first run
2. **Connection String Warning**: EF context has hardcoded fallback (see #warning in code)
3. **Integration Tests**: Use Docker/Testcontainers, may require Docker Desktop
4. **PowerShell**: Use semicolon (`;`) instead of `&&` for command chaining

## Architecture Patterns and Conventions

### Backend (.NET)
- **Clean Architecture**: Domain ? Application ? Infrastructure ? API
- **Authentication**: JWT Bearer tokens with ASP.NET Core Identity
- **API Versioning**: Uses Asp.Versioning with v1 endpoints
- **Validation**: Built-in ASP.NET Core model validation
- **CORS**: Configured for localhost:4201 in DEBUG mode
- **Dependency Injection**: Standard ASP.NET Core DI container

### Frontend (Angular)
- **State Management**: NgRx Store with effects
- **UI Framework**: Angular Material
- **Styling**: SCSS with custom themes (4 theme files)
- **HTTP**: Standard Angular HttpClient with interceptors
- **Routing**: Angular Router with guards
- **Testing**: Component tests with TestBed

### Code Quality Standards
- **EditorConfig**: Comprehensive formatting rules in `.editorconfig`
- **ESLint**: Strict TypeScript and Angular rules
- **Analyzer Rules**: Multiple CA and IDE diagnostics enabled
- **File-scoped namespaces**: Preferred in C#
- **Nullable reference types**: Enabled

## Important Notes for Agents

1. **Trust these instructions** - All commands have been validated and work correctly
2. **Database dependency** - Most operations require SQL Server and proper connection string
3. **Build order matters** - Always build backend before running integration tests
4. **Certificate setup** - Always run certificate commands for HTTPS to work
5. **Working directory** - API commands from repo root, UI commands from `src/WarehouseEngine.UI`
6. **Test containers** - Integration tests use Docker, ensure Docker is available
7. **Configuration** - appsettings.local.json is required and gitignored for security

Only search for additional information if these instructions are incomplete or incorrect. The solution structure, build commands, and validation steps documented here are current and tested.