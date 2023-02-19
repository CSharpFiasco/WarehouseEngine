using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WarehouseEngine.Application.Interfaces;
using WarehouseEngine.Domain.Entities;

namespace WarehouseEngine.Infrastructure.DataContext;

[ExcludeFromCodeCoverage]
public partial class WarehouseEngineContext : IdentityDbContext<IdentityUser>, IWarehouseEngineContext
{
    public WarehouseEngineContext()
    {
    }

    public WarehouseEngineContext(DbContextOptions<WarehouseEngineContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Company> Company { get; set; } = null!;
    public virtual DbSet<Contact> Contact { get; set; } = null!;
    public virtual DbSet<Customer> Customer { get; set; } = null!;
    public virtual DbSet<Employee> Employee { get; set; } = null!;
    public virtual DbSet<Item> Item { get; set; } = null!;
    public virtual DbSet<Order> Order { get; set; } = null!;
    public virtual DbSet<OrderWarehouseItem> OrderWarehouseItem { get; set; } = null!;
    public virtual DbSet<OrderWarehouseItemOutOfStock> OrderWarehouseItemOutOfStock { get; set; } = null!;
    public virtual DbSet<Position> Position { get; set; } = null!;
    public virtual DbSet<PurchaseOrder> PurchaseOrder { get; set; } = null!;
    public virtual DbSet<PurchaseOrderWarehouseItem> PurchaseOrderWarehouseItem { get; set; } = null!;
    public virtual DbSet<Vendor> Vendor { get; set; } = null!;
    public virtual DbSet<Warehouse> Warehouse { get; set; } = null!;
    public virtual DbSet<WarehouseItem> WarehouseItem { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Contact>(entity =>
        {
            entity.OwnsOne(d => d.Address);
        });

        builder.Entity<Customer>(entity =>
        {
            var billingAddress = entity.OwnsOne(d => d.BillingAddress);
            billingAddress.Property(a => a.Address1).HasColumnName("BillingAddress1");
            billingAddress.Property(a => a.Address2).HasColumnName("BillingAddress2");
            billingAddress.Property(a => a.City).HasColumnName("BillingCity");
            billingAddress.Property(a => a.State).HasColumnName("BillingState");
            billingAddress.Property(a => a.ZipCode).HasColumnName("BillingZipCode");

            var shippingAddress = entity.OwnsOne(d => d.ShippingAddress);
            shippingAddress.Property(a => a.Address1).HasColumnName("ShippingAddress1");
            shippingAddress.Property(a => a.Address2).HasColumnName("ShippingAddress2");
            shippingAddress.Property(a => a.City).HasColumnName("ShippingCity");
            shippingAddress.Property(a => a.State).HasColumnName("ShippingState");
            shippingAddress.Property(a => a.ZipCode).HasColumnName("ShippingZipCode");

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

        builder.Entity<Employee>(entity =>
        {
            entity.Property(e => e.SocialSecuritySerialNumber).IsFixedLength();

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

        builder.Entity<Order>(entity =>
        {
            entity.HasOne(d => d.Customer)
                .WithMany(p => p.Order)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Customer");

            var shippingAddress = entity.OwnsOne(d => d.ShippingAddress);
            shippingAddress.Property(a => a.Address1).HasColumnName("ShippingAddress1");
            shippingAddress.Property(a => a.Address2).HasColumnName("ShippingAddress2");
            shippingAddress.Property(a => a.City).HasColumnName("ShippingCity");
            shippingAddress.Property(a => a.State).HasColumnName("ShippingState");
            shippingAddress.Property(a => a.ZipCode).HasColumnName("ShippingZipCode");
        });

        builder.Entity<OrderWarehouseItem>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.WarehouseItemId });

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

        builder.Entity<OrderWarehouseItemOutOfStock>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.WarehouseItemId });

            entity.HasOne(d => d.Order)
                .WithMany(p => p.OrderWarehouseItemOutOfStock)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderWarehouseItemOutOfStock_Order");

            entity.HasOne(d => d.WarehouseItem)
                .WithMany(p => p.OrderWarehouseItemOutOfStock)
                .HasForeignKey(d => d.WarehouseItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderWarehouseItemOutOfStock_WarehouseItemOutOfStock");
        });

        builder.Entity<PurchaseOrderWarehouseItem>(entity =>
        {
            entity.HasKey(e => new { e.PurchaseOrderId, e.WarehouseItemId });

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

        builder.Entity<Vendor>(entity =>
        {
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

        builder.Entity<WarehouseItem>(entity =>
        {
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
