using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WarehouseEngine.Application.Interfaces;
using WarehouseEngine.Domain.Models.Login;

namespace WarehouseEngine.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class AuthenticateController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IJwtService _jwtService;
    private readonly HttpContext _httpContext;
    public AuthenticateController(
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IJwtService jwtService,
        HttpContext httpContext)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtService = jwtService;
        _httpContext = httpContext;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] Login model)
    {
        IdentityUser? user = await _userManager.FindByNameAsync(model.Username);
        if (user is not null && user.UserName is not null && await _userManager.CheckPasswordAsync(user, model.Password))
        {
            IList<string> userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

            foreach (string userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            string token = _jwtService.GetNewToken(authClaims);
            Response.Headers.Add("Bearer", token);

            return Ok();
        }
        return Unauthorized();
    }

    [HttpPost]
    public async Task<IActionResult> RefreshToken()
    {
        //user
        _httpContext.Request.Headers.TryGetValue("Authorization", out var headerAuth);
        var token = headerAuth.FirstOrDefault();

        if (token == null)
        {
            //return
        }
    }
}
