using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseEngine.Domain.ValidationResults;
public sealed class InvalidShippingResult() : ValidationResult(GetDefaultMessage())
{
    public static string GetDefaultMessage()
    {
        return $"Shipping is missing";
    }
}
