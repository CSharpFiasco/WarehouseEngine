using WarehouseEngine.Domain.ValueObjects;

namespace WarehouseEngine.Domain.Entities;

public partial class Contact
{
    public const int MaxNameLength = 80;
    public const int MaxEmailLength = 60;

    public static Contact Create(Guid id, string? firstName = null, string? lastName = null,
        string? email = null, Address? address = null)
    {
        EntityRules.Identity(id, nameof(id));
        var contact = new Contact { Id = id };
        contact.UpdateDetails(firstName, lastName, email, address);
        return contact;
    }

    public void UpdateDetails(string? firstName, string? lastName, string? email, Address? address)
    {
        EntityRules.OptionalText(firstName, MaxNameLength, nameof(firstName));
        EntityRules.OptionalText(lastName, MaxNameLength, nameof(lastName));
        EntityRules.OptionalText(email, MaxEmailLength, nameof(email));
        if (address is not null)
            EntityRules.Address(address, nameof(address));

        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Address = address;
    }
}
