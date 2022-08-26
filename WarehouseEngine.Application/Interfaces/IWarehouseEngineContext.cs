using Microsoft.EntityFrameworkCore;
using WarehouseEngine.Core.Entities;

namespace WarehouseEngine.Application.Interfaces;
public interface IWarehouseEngineContext
{
    DbSet<Address> Address { get; set; }
    DbSet<Company> Company { get; set; }
    DbSet<Contact> Contact { get; set; }
    DbSet<Customer> Customer { get; set; }
    DbSet<Employee> Employee { get; set; }
    DbSet<Item> Item { get; set; }
    DbSet<Order> Order { get; set; }
    DbSet<OrderWarehouseItem> OrderWarehouseItem { get; set; }
    DbSet<Position> Position { get; set; }
    DbSet<PurchaseOrder> PurchaseOrder { get; set; }
    DbSet<PurchaseOrderWarehouseItem> PurchaseOrderWarehouseItem { get; set; }
    DbSet<Vendor> Vendor { get; set; }
    DbSet<Warehouse> Warehouse { get; set; }
    DbSet<WarehouseItem> WarehouseItem { get; set; }

    Task<int> SaveChangesAsync();
}
