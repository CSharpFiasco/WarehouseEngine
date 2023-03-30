using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WarehouseEngine.Api.Configuration;
public class ConfigureSwaggerGenOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _apiVersionDescriptionProvider;
    public ConfigureSwaggerGenOptions(IApiVersionDescriptionProvider apiVersionDescriptionProvider)
    {
        _apiVersionDescriptionProvider = apiVersionDescriptionProvider;
    }

    public void Configure(SwaggerGenOptions options)
    {
        // add a swagger document for each discovered API version
        foreach (var apiVersionDescription in _apiVersionDescriptionProvider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(apiVersionDescription.GroupName, CreateInfoForApiVersion(apiVersionDescription));
        }
    }

    static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
    {
        var info = new OpenApiInfo()
        {
            Title = "Warehouse Engine",
            Version = description.ApiVersion.ToString()
        };

        if (description.IsDeprecated)
        {
            info.Description = "This API version has been deprecated.";
        }

        return info;
    }
}
