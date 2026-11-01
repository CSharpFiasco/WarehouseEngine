var builder = DistributedApplication.CreateBuilder(args);

var database = builder.AddSqlServer("sql")
    .WithDataVolume()
    .AddDatabase("WarehouseEngine");

var migrations = builder.AddProject<Projects.WarehouseEngine_MigrationService>("migrations")
    .WithReference(database)
    .WaitFor(database);

var api = builder.AddProject<Projects.WarehouseEngine_Api>("api")
    .WithReference(database)
    .WaitForCompletion(migrations);

builder.AddJavaScriptApp("ui", "../WarehouseEngine.UI", "start:vs")
    .WithHttpEndpoint(port: 4201, targetPort: 4201, isProxied: false)
    .WithReference(api)
    .WaitFor(api);

await builder.Build().RunAsync();