//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using EfSchemaCompare;
//using WarehouseEngine.Infrastructure.DataContext;
//using Xunit.Abstractions;

//namespace WarehouseEngine.Infrastructure.Tests;
//public class SchemaDbCompareTests : IClassFixture<TestDatabaseFixture>
//{
//    private readonly TestDatabaseFixture _fixture;
//    private readonly ITestOutputHelper _output;
//    public SchemaDbCompareTests(TestDatabaseFixture fixture, ITestOutputHelper output)
//    {
//        _fixture = fixture;
//        _output = output;
//    }

    //[Fact]
    //public async Task CompareViaContext()
    //{
    //    await using WarehouseEngineContext context = _fixture.CreateContext();
    //        var comparer = new CompareEfSql();

    //    //ATTEMPT
    //    //This will compare EF Core model of the database with the database that the context's connection points to
    //    var hasErrors = comparer.CompareEfWithDb(context);
    //    if (hasErrors)
    //    {
    //        _output.WriteLine(comparer.GetAllErrors);
    //    }

    //    Assert.False(hasErrors);
    //}
//}
