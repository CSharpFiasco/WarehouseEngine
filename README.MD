# Markdown File

[Database-first](https://www.entityframeworktutorial.net/efcore/create-model-for-existing-database-in-ef-core.aspx) solutions assume the existence of a database. 

This is useful when trying to build an application based off of an existing database, for example, when rewriting an application or when performance is crucial, etc.

Please deploy this the database project of your choice then place the following in a appsettings.local.json file inside the [Api](WarehouseEngine.Api) project.
```
{
  "ConnectionStrings": {
    "WarehouseEngine": "Server=(localdb)\\MSSQLLocalDB;Database=WarehouseEngine;Trusted_Connection=True"
  }
}
```

Because we use EF Core, a repo layer does not faciliate testing since we can test against MSSQLLOCALDB. This blurs the line of unit and integration tests, but provides value quicker.

May need to do following commands

`dotnet dev-certs https --clean`

`dotnet dev-certs https --trust`

`dotnet dev-certs https --check`

## todo

1. Add global exception handling