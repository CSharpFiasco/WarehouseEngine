using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseEngine.Domain.ValidationResults;
public sealed class InvalidShippingResult() : ValidationResult(GetDefaultMessage())
{
    private static readonly string _defaultMessage = "Shipping is missing";
    public static string GetDefaultMessage() => _defaultMessage;
}
