using System.ComponentModel.DataAnnotations;
using WarehouseEngine.Domain.ValueObjects;

namespace WarehouseEngine.Domain.Entities;

internal static class EntityRules
{
    public static void Identity(Guid value, string parameterName)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("Identity must not be empty.", parameterName);
    }

    public static void RequiredText(string value, int maxLength, string parameterName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value, parameterName);
        OptionalText(value, maxLength, parameterName);
    }

    public static void OptionalText(string? value, int maxLength, string parameterName)
    {
        if (value?.Length > maxLength)
            throw new ArgumentOutOfRangeException(parameterName, $"Value must not exceed {maxLength} characters.");
    }

    public static void Address(Address value, string parameterName)
    {
        ArgumentNullException.ThrowIfNull(value, parameterName);
        var errors = new List<ValidationResult>();
        if (!Validator.TryValidateObject(value, new ValidationContext(value), errors, true))
            throw new ArgumentException(string.Join(" ", errors.Select(e => e.ErrorMessage)), parameterName);
    }

    public static void Stock(int quantity, decimal? price)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(quantity);
        if (price < 0)
            throw new ArgumentOutOfRangeException(nameof(price), "Price must not be negative.");
    }

    public static void LineQuantity(int quantity) =>
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
}
