using System.Text.Json;
using System.Text.Json.Nodes;
using WarehouseEngine.Application.Dtos;
using WarehouseEngine.Domain.Entities;

namespace WarehouseEngine.Api.Examples;
public static class ExampleDictionary
{
    public static readonly IReadOnlyDictionary<Type, JsonNode?> Examples = new Dictionary<Type, JsonNode?>
    {
        { typeof(CustomerResponseDto), JsonSerializer.SerializeToNode(CustomerExamples.CustomerResponseDto, JsonSerializerOptions.Web) },
        { typeof(VendorResponseDto), JsonSerializer.SerializeToNode(VendorExamples.VendorResponseDto, JsonSerializerOptions.Web) },
        { typeof(AuthenticationResponse), JsonSerializer.SerializeToNode(AuthenticationExamples.AuthenticationResultExample, JsonSerializerOptions.Web)  }
    };
}
