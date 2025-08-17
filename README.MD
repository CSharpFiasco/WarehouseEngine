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

Because we use EF Core, a repo layer does not facilitate testing since we can test against MSSQLLOCALDB by creating a real instance of the db.

Additionally, when we write integration tests, we can test against a real db instance which is spun up via TestContainers when the test begins.