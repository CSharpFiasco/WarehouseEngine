using System.Security.Claims;

namespace WarehouseEngine.Application.Interfaces;
public interface IJwtService
{
    string GetNewToken(List<Claim> authClaims);
}
