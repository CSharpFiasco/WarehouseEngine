using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarehouseEngine.Application.Interfaces;
using WarehouseEngine.Domain.Models.Login;

namespace WarehouseEngine.Api.Controllers;

[ApiController]
[AllowAnonymous]
public class AuthenticateController : Controller
{
    private readonly IJwtService _jwtService;
    public AuthenticateController(IJwtService jwtService)
    {
        _jwtService = jwtService;
    }

    [HttpPost]
    [Route("login")]
    public IActionResult Login([FromBody] Login model)
    {
        var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "MyName"),
                    new Claim(ClaimTypes.Role, UserRoles.User),
                    new Claim(ClaimTypes.Role, UserRoles.Admin)
                };
        var token = _jwtService.GetNewToken(authClaims);

        Response.Headers.Add("Bearer", token);

        return Ok();
    }
}
