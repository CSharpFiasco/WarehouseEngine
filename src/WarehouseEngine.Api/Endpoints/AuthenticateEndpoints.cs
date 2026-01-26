using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using WarehouseEngine.Application.Dtos;
using WarehouseEngine.Application.Interfaces;
using WarehouseEngine.Domain.Models.Auth;

namespace WarehouseEngine.Api.Endpoints;

internal class AuthenticateEndpoints
{
    protected AuthenticateEndpoints() { }

    public static void Map(WebApplication app)
    {
        app.MapPost("/api/v{version:apiVersion}/authenticate", [AllowAnonymous] async (
            UserManager<IdentityUser> userManager,
            IJwtService jwtService,
            Login model,
            HttpContext httpContext) =>
        {
            IdentityUser? user = await userManager.FindByNameAsync(model.Username);
            if (user is not null && user.UserName is not null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                IList<string> userRoles = await userManager.GetRolesAsync(user);

                List<Claim> authClaims =
                [
                    new Claim(WarehouseClaimTypes.UserId, user.Id),
                    new Claim(WarehouseClaimTypes.Name, user.UserName),
                ];

                foreach (string userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                string token = jwtService.GetNewToken(authClaims);
                httpContext.Response.Headers.Append("Bearer", token);

                return Results.Ok(new AuthenticationResponse(token));
            }
            return Results.Unauthorized();
        })
        .Produces<AuthenticationResponse>(200)
        .Produces(401)
        .Produces(500)
        .WithName("Login")
        .WithTags("Authenticate");
    }
}
