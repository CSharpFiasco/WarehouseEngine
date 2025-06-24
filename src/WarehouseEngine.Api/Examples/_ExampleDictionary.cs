using System.Text.Json;
using Microsoft.OpenApi.Any;
using WarehouseEngine.Domain.Entities;

namespace WarehouseEngine.Api.Examples;
public static class ExampleDictionary
{
    public static readonly IReadOnlyDictionary<Type, OpenApiString> Examples = new Dictionary<Type, OpenApiString>
    {
        { typeof(CustomerResponseDto),  new OpenApiString(JsonSerializer.Serialize(CustomerExamples.CustomerResponseDto, JsonSerializerOptions.Web))},
    };
}
