

namespace WarehouseEngine.Domain.Entities;

public partial class Customer
{
    public Customer()
    {
        Order = new HashSet<Order>();
        Contact = new HashSet<Contact>();
    }

    public int Id { get; set; }
    public required string Name { get; set; }
    public int AddressId { get; set; }

    public virtual Address? Address { get; set; }
    public virtual ICollection<Order> Order { get; set; }

    public virtual ICollection<Contact> Contact { get; set; }
}
