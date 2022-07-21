namespace WarehouseEngine.Core.Entities;

public partial class Address
{
    public Address()
    {
        Contact = new HashSet<Contact>();
        Customer = new HashSet<Customer>();
        Order = new HashSet<Order>();
    }

    public int Id { get; set; }
    public string Address1 { get; set; } = null!;
    public string Address2 { get; set; } = null!;
    public string City { get; set; } = null!;
    public string State { get; set; } = null!;
    public string Zip { get; set; } = null!;

    public virtual ICollection<Contact> Contact { get; set; }
    public virtual ICollection<Customer> Customer { get; set; }
    public virtual ICollection<Order> Order { get; set; }
}
