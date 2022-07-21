namespace WarehouseEngine.Core.Entities;

public partial class Contact
{
    public Contact()
    {
        Customer = new HashSet<Customer>();
    }

    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public int? AddressId { get; set; }

    public virtual Address? Address { get; set; }

    public virtual ICollection<Customer> Customer { get; set; }
}
