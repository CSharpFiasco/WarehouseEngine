using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace WarehouseEngine.Api.Middleware.Auth;
public class WarehouseClaimsTransformation: IClaimsTransformation
{
    public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var identity = principal.Identity;
        if (identity is null) return Task.FromResult(principal);

        var sub = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        if (sub is null) return Task.FromResult(principal);

        // todo: add tenant ids to claims
        Console.WriteLine($"User {sub} is authenticated");

        return Task.FromResult(principal);
    }
}
