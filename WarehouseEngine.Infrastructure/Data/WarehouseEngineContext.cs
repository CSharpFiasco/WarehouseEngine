using Microsoft.EntityFrameworkCore;
using WarehouseEngine.Application.Interfaces;
using WarehouseEngine.Domain.Entities;

namespace WarehouseEngine.Infrastructure.Data;

public partial class WarehouseEngineContext : DbContext, IWarehouseEngineContext
{
    public WarehouseEngineContext(DbContextOptions<WarehouseEngineContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Address { get; set; } = null!;
    public virtual DbSet<Company> Company { get; set; } = null!;
    public virtual DbSet<Contact> Contact { get; set; } = null!;
    public virtual DbSet<Customer> Customer { get; set; } = null!;
    public virtual DbSet<Employee> Employee { get; set; } = null!;
    public virtual DbSet<Item> Item { get; set; } = null!;
    public virtual DbSet<Order> Order { get; set; } = null!;
    public virtual DbSet<OrderWarehouseItem> OrderWarehouseItem { get; set; } = null!;
    public virtual DbSet<Position> Position { get; set; } = null!;
    public virtual DbSet<PurchaseOrder> PurchaseOrder { get; set; } = null!;
    public virtual DbSet<PurchaseOrderWarehouseItem> PurchaseOrderWarehouseItem { get; set; } = null!;
    public virtual DbSet<Vendor> Vendor { get; set; } = null!;
    public virtual DbSet<Warehouse> Warehouse { get; set; } = null!;
    public virtual DbSet<WarehouseItem> WarehouseItem { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.Property(e => e.Address1)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("Address");

            entity.Property(e => e.Address2)
                .HasMaxLength(80)
                .IsUnicode(false);

            entity.Property(e => e.City)
                .HasMaxLength(32)
                .IsUnicode(false);

            entity.Property(e => e.State)
                .HasMaxLength(2)
                .IsUnicode(false);

            entity.Property(e => e.Zip)
                .HasMaxLength(11)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.Property(e => e.Name)
                .HasMaxLength(80)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Contact>(entity =>
        {
            entity.HasIndex(e => e.AddressId, "IX_Contact01");

            entity.Property(e => e.Email)
                .HasMaxLength(60)
                .IsUnicode(false);

            entity.Property(e => e.FirstName)
                .HasMaxLength(80)
                .IsUnicode(false);

            entity.Property(e => e.LastName)
                .HasMaxLength(80)
                .IsUnicode(false);

            entity.HasOne(d => d.Address)
                .WithMany(p => p.Contact)
                .HasForeignKey(d => d.AddressId)
                .HasConstraintName("FK_Contact_Address");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasIndex(e => e.AddressId, "IX_Customer01");

            entity.Property(e => e.Name)
                .HasMaxLength(80)
                .IsUnicode(false);

            entity.HasOne(d => d.Address)
                .WithMany(p => p.Customer)
                .HasForeignKey(d => d.AddressId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Customer_Address");

            entity.HasMany(d => d.Contact)
                .WithMany(p => p.Customer)
                .UsingEntity<Dictionary<string, object>>(
                    "CustomerContact",
                    l => l.HasOne<Contact>().WithMany().HasForeignKey("ContactId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_CustomerContact_Contact"),
                    r => r.HasOne<Customer>().WithMany().HasForeignKey("CustomerId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_CustomerContact_Customer"),
                    j =>
                    {
                        j.HasKey("CustomerId", "ContactId");

                        j.ToTable("CustomerContact");

                        j.HasIndex(new[] { "CustomerId" }, "IX_CustomerContact01");

                        j.HasIndex(new[] { "ContactId" }, "IX_CustomerContact02");
                    });
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasIndex(e => e.PositionId, "IX_Employee01");

            entity.HasIndex(e => e.SupervisorEmployeeId, "IX_Employee02");

            entity.Property(e => e.FirstName)
                .HasMaxLength(30)
                .IsUnicode(false);

            entity.Property(e => e.LastName)
                .HasMaxLength(30)
                .IsUnicode(false);

            entity.Property(e => e.MiddleName)
                .HasMaxLength(30)
                .IsUnicode(false);

            entity.Property(e => e.SocialSecurityNumberHash).HasMaxLength(32);

            entity.Property(e => e.SocialSecuritySerialNumber)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.UserName)
                .HasMaxLength(32)
                .IsUnicode(false);

            entity.HasOne(d => d.Position)
                .WithMany(p => p.Employee)
                .HasForeignKey(d => d.PositionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employee_Position");

            entity.HasOne(d => d.SupervisorEmployee)
                .WithMany(p => p.InverseSupervisorEmployee)
                .HasForeignKey(d => d.SupervisorEmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employee_SupervisorEmployee");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.Property(e => e.Description)
                .HasMaxLength(120)
                .IsUnicode(false);

            entity.Property(e => e.Sku)
                .HasMaxLength(12)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasIndex(e => e.AddressId, "IX_Order01");

            entity.HasOne(d => d.Address)
                .WithMany(p => p.Order)
                .HasForeignKey(d => d.AddressId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Address");

            entity.HasOne(d => d.Customer)
                .WithMany(p => p.Order)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Customer");

            entity.HasMany(d => d.WarehouseItem)
                .WithMany(p => p.Order)
                .UsingEntity<Dictionary<string, object>>(
                    "OrderWarehouseItemOutOfStock",
                    l => l.HasOne<WarehouseItem>().WithMany().HasForeignKey("WarehouseItemId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_OrderWarehouseItemOutOfStock_WarehouseItemOutOfStock"),
                    r => r.HasOne<Order>().WithMany().HasForeignKey("OrderId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_OrderWarehouseItemOutOfStock_Order"),
                    j =>
                    {
                        j.HasKey("OrderId", "WarehouseItemId");

                        j.ToTable("OrderWarehouseItemOutOfStock");

                        j.HasIndex(new[] { "OrderId" }, "IX_OrderWarehouseItemOutOfStock01");

                        j.HasIndex(new[] { "WarehouseItemId" }, "IX_OrderWarehouseItemOutOfStock02");
                    });
        });

        modelBuilder.Entity<OrderWarehouseItem>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.WarehouseItemId });

            entity.HasIndex(e => e.OrderId, "IX_OrderWarehouseItem01");

            entity.HasIndex(e => e.WarehouseItemId, "IX_OrderWarehouseItem02");

            entity.HasOne(d => d.Order)
                .WithMany(p => p.OrderWarehouseItem)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderWarehouseItem_Order");

            entity.HasOne(d => d.WarehouseItem)
                .WithMany(p => p.OrderWarehouseItem)
                .HasForeignKey(d => d.WarehouseItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderWarehouseItem_Warehouse");
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.Property(e => e.Name)
                .HasMaxLength(80)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PurchaseOrder>(entity =>
        {
            entity.Property(e => e.OrderDate).HasColumnType("date");

            entity.Property(e => e.OrderNumber)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PurchaseOrderWarehouseItem>(entity =>
        {
            entity.HasKey(e => new { e.PurchaseOrderId, e.WarehouseItemId });

            entity.HasIndex(e => e.PurchaseOrderId, "IX_PurchaseOrderWarehouseItem01");

            entity.HasIndex(e => e.WarehouseItemId, "IX_PurchaseOrderWarehouseItem02");

            entity.HasOne(d => d.PurchaseOrder)
                .WithMany(p => p.PurchaseOrderWarehouseItem)
                .HasForeignKey(d => d.PurchaseOrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchaseOrderWarehouseItem_PurchaseOrder");

            entity.HasOne(d => d.WarehouseItem)
                .WithMany(p => p.PurchaseOrderWarehouseItem)
                .HasForeignKey(d => d.WarehouseItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchaseOrderWarehouseItem_WarehouseItem");
        });

        modelBuilder.Entity<Vendor>(entity =>
        {
            entity.Property(e => e.Name)
                .HasMaxLength(80)
                .IsUnicode(false);

            entity.HasMany(d => d.Item)
                .WithMany(p => p.Vendor)
                .UsingEntity<Dictionary<string, object>>(
                    "VendorItem",
                    l => l.HasOne<Item>().WithMany().HasForeignKey("ItemId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_VendorItem_Item"),
                    r => r.HasOne<Vendor>().WithMany().HasForeignKey("VendorId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_VendorItem_Vendor"),
                    j =>
                    {
                        j.HasKey("VendorId", "ItemId");

                        j.ToTable("VendorItem");

                        j.HasIndex(new[] { "VendorId" }, "IX_VendorItem01");

                        j.HasIndex(new[] { "ItemId" }, "IX_VendorItem02");
                    });
        });

        modelBuilder.Entity<Warehouse>(entity =>
        {
            entity.Property(e => e.Name)
                .HasMaxLength(32)
                .IsUnicode(false);
        });

        modelBuilder.Entity<WarehouseItem>(entity =>
        {
            entity.HasIndex(e => e.WarehouseId, "IX_WarehouseItem01");

            entity.HasIndex(e => e.ItemId, "IX_WarehouseItem02");

            entity.Property(e => e.Price).HasColumnType("decimal(15, 3)");

            entity.HasOne(d => d.Item)
                .WithMany(p => p.WarehouseItem)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WarehouseItem_Item");

            entity.HasOne(d => d.Warehouse)
                .WithMany(p => p.WarehouseItem)
                .HasForeignKey(d => d.WarehouseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WarehouseItem_Warehouse");
        });
    }

    public Task<int> SaveChangesAsync() => base.SaveChangesAsync();
}
