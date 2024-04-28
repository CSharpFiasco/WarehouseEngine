using System.ComponentModel.DataAnnotations;

namespace WarehouseEngine.Domain.ValidationResults;

public class GenericValidationResult(string message) : ValidationResult(message)
{

}
