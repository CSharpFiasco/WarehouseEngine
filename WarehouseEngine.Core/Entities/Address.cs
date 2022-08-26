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
    public required string Address1 { get; set; }
    public required string Address2 { get; set; }
    public required string City { get; set; }
    public required string State { get; set; }
    public required string Zip { get; set; }

    public virtual ICollection<Contact> Contact { get; set; }
    public virtual ICollection<Customer> Customer { get; set; }
    public virtual ICollection<Order> Order { get; set; }
}
