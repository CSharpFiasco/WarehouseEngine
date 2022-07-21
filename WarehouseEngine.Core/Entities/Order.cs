﻿namespace WarehouseEngine.Core.Entities;

public partial class Order
{
    public Order()
    {
        OrderWarehouseItem = new HashSet<OrderWarehouseItem>();
        WarehouseItem = new HashSet<WarehouseItem>();
    }

    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int AddressId { get; set; }
    public byte Status { get; set; }

    public virtual Address Address { get; set; } = null!;
    public virtual Customer Customer { get; set; } = null!;
    public virtual ICollection<OrderWarehouseItem> OrderWarehouseItem { get; set; }

    public virtual ICollection<WarehouseItem> WarehouseItem { get; set; }
}
