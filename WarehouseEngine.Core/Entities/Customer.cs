

namespace WarehouseEngine.Core.Entities;

public partial class Customer
{
    public Customer()
    {
        Order = new HashSet<Order>();
        Contact = new HashSet<Contact>();
    }

    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int AddressId { get; set; }

    public virtual Address Address { get; set; } = null!;
    public virtual ICollection<Order> Order { get; set; }

    public virtual ICollection<Contact> Contact { get; set; }
}
