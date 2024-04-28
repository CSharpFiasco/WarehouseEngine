using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseEngine.Domain.Models.Auth;
public static class WarehouseClaimTypes
{
    public static string Name => ClaimTypes.Name;
    /// <summary>
    /// Maps to the <see cref="ClaimTypes.NameIdentifier"/> claim type
    /// </summary>
    public static string UserId => ClaimTypes.NameIdentifier;
}
