# CI/CD Workflows for WarehouseEngine

This repository includes several GitHub Actions workflows for continuous integration and deployment.

## Workflows

### 1. CI Build and Test (`ci.yml`)
**Triggers:** Push/PR to `main` or `develop` branches

**What it does:**
- Sets up .NET 9.0 environment
- Provides SQL Server service for database tests
- Builds all core projects (excluding Database project which requires .NET Framework)
- Runs all test suites:
  - Domain tests (always runs)
  - Infrastructure tests (with SQL Server, allows failures)
  - API Integration tests (allows failures if Docker issues)
- Publishes the API project
- Uploads code coverage reports to Codecov

**Projects built:**
- WarehouseEngine.Domain
- WarehouseEngine.Application  
- WarehouseEngine.Infrastructure
- WarehouseEngine.Api
- All test projects

### 2. API Build and Test (`api-ci.yml`)
**Triggers:** Push/PR to `main` or `develop` branches, but only when API-related files change

**What it does:**
- Focused on API project specifically
- Builds and tests only API-related components
- Creates deployable API artifacts
- Optimized for faster feedback on API changes

### 3. Build Only (`build-only.yml`)
**Triggers:** Manual dispatch or daily at 2 AM UTC

**What it does:**
- Simple build verification
- Tests API publishing capability
- No tests, just compilation verification
- Useful for dependency updates or general health checks

## Features

### SQL Server Integration
The main CI workflow includes a SQL Server 2022 service for Infrastructure tests that require database connectivity.

### Error Handling
- Infrastructure tests allow failures (SQL Server connectivity issues)
- API Integration tests allow failures (Docker environment issues)
- Build continues even if some tests fail to provide maximum feedback

### Code Coverage
All test runs collect XPlat Code Coverage and upload to Codecov (requires `CODECOV_TOKEN` secret).

### Artifact Management
- API builds are uploaded as artifacts with 7-day retention
- Published API output is available for deployment

## Setup Requirements

### Secrets (Optional)
- `CODECOV_TOKEN` - For code coverage reporting to Codecov

### Environment
- Requires GitHub Actions runners with:
  - .NET 9.0 support
  - Docker support (for API Integration tests)
  - SQL Server service support

## Project Dependencies

The build excludes the `WarehouseEngine.Database` project as it requires .NET Framework 4.7.2, which is not available in the Linux CI environment. This is intentional and does not affect the API functionality.

## Usage

1. **Automatic Builds**: Push or create PR to `main`/`develop` branches
2. **Manual Builds**: Use "Actions" tab → "Build Only" → "Run workflow"
3. **API-specific Builds**: Automatically triggered when API-related files change

## Troubleshooting

### Common Issues
1. **Infrastructure tests failing**: Check SQL Server service health
2. **API Integration tests failing**: Usually Docker-related, workflow continues
3. **Database project build errors**: Expected and excluded from CI

### Local Testing
To test the build locally:
```bash
dotnet restore
dotnet build src/WarehouseEngine.Api/WarehouseEngine.Api.csproj --configuration Release
dotnet test tests/WarehouseEngine.Domain.Tests/WarehouseEngine.Domain.Tests.csproj
dotnet publish src/WarehouseEngine.Api/WarehouseEngine.Api.csproj --output ./dist
```