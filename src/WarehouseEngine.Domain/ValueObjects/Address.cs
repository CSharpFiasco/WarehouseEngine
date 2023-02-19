using WarehouseEngine.Domain.Helpers;

namespace WarehouseEngine.Domain.ValueObjects;

public class Address : ValueObject
{
    public string Address1 { get; private set; }
    public string? Address2 { get; private set; }
    public string City { get; private set; }
    public string State { get; private set; }
    public string ZipCode { get; private set; }

    public Address(string address1, string? address2, string city, string state, string zipCode)
    {
        Address1 = address1;
        Address2 = address2;
        City = city;
        State = state;
        ZipCode = zipCode;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        // Using a yield return statement to return each element one at a time
        yield return Address1;
        yield return Address2;
        yield return City;
        yield return State;
        yield return ZipCode;
    }
}
