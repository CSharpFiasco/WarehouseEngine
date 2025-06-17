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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
            optionsBuilder.UseSqlServer($"Server=localhost;Database=WarehouseEngine;Trusted_Connection=True;MultipleActiveResultSets=true");
        }

        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
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

    public virtual DbSet<VendorItem> VendorItem { get; set; } = null!;

    public virtual DbSet<Warehouse> Warehouse { get; set; } = null!;
    public virtual DbSet<WarehouseItem> WarehouseItem { get; set; } = null!;

    public virtual DbSet<WarehousePickList> WarehousePickList { get; set; } = null!;

    public virtual DbSet<WarehousePickListItem> WarehousePickListItem { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Company>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        builder.Entity<Contact>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.OwnsOne(d => d.Address, (e) =>
            {
                e.Property(a => a.Address1).HasColumnName("Address1").IsRequired();
                e.Property(a => a.Address2).HasColumnName("Address2");
                e.Property(a => a.City).HasColumnName("City").IsRequired();
                e.Property(a => a.State).HasColumnName("State").IsRequired();
                e.Property(a => a.ZipCode).HasColumnName("ZipCode").IsRequired();
            });
        });

        builder.Entity<Customer>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();

            var billingAddress = entity.OwnsOne(d => d.BillingAddress);
            billingAddress.Property(a => a.Address1).HasColumnName("BillingAddress1").IsRequired();
            billingAddress.Property(a => a.Address2).HasColumnName("BillingAddress2");
            billingAddress.Property(a => a.City).HasColumnName("BillingCity").IsRequired();
            billingAddress.Property(a => a.State).HasColumnName("BillingState").IsRequired();
            billingAddress.Property(a => a.ZipCode).HasColumnName("BillingZipCode").IsRequired();

            var shippingAddress = entity.OwnsOne(d => d.ShippingAddress);
            shippingAddress.Property(a => a.Address1).HasColumnName("ShippingAddress1").IsRequired();
            shippingAddress.Property(a => a.Address2).HasColumnName("ShippingAddress2");
            shippingAddress.Property(a => a.City).HasColumnName("ShippingCity").IsRequired();
            shippingAddress.Property(a => a.State).HasColumnName("ShippingState").IsRequired();
            shippingAddress.Property(a => a.ZipCode).HasColumnName("ShippingZipCode").IsRequired();


            entity.HasMany(d => d.Contact)
                .WithMany(p => p.Customer)
                .UsingEntity<Dictionary<string, object>>(
                    "CustomerContact",
                    r => r.HasOne<Contact>().WithMany()
                        .HasForeignKey("ContactId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_CustomerContact_Contact"),
                    l => l.HasOne<Customer>().WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_CustomerContact_Customer"),
                    j =>
                    {
                        j.HasKey("CustomerId", "ContactId");
                        j.HasIndex(new[] { "CustomerId" }, "IX_CustomerContact01");
                        j.HasIndex(new[] { "ContactId" }, "IX_CustomerContact02");
                    });
        });

        builder.Entity<Employee>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
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

        builder.Entity<Item>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        builder.Entity<Order>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Customer)
                .WithMany(p => p.Order)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Customer");

            var shippingAddress = entity.OwnsOne(d => d.ShippingAddress);
            shippingAddress.Property(a => a.Address1).HasColumnName("ShippingAddress1").IsRequired();
            shippingAddress.Property(a => a.Address2).HasColumnName("ShippingAddress2");
            shippingAddress.Property(a => a.City).HasColumnName("ShippingCity").IsRequired();
            shippingAddress.Property(a => a.State).HasColumnName("ShippingState").IsRequired();
            shippingAddress.Property(a => a.ZipCode).HasColumnName("ShippingZipCode").IsRequired();
        });

        builder.Entity<OrderWarehouseItem>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.WarehouseItemId });
            entity.HasOne(d => d.Order)
                .WithMany(p => p.OrderWarehouseItem)
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

        builder.Entity<Position>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        builder.Entity<PurchaseOrder>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
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
            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasMany(d => d.VendorItem)
                  .WithOne(p => p.Vendor)
                  .HasForeignKey(d => d.VendorId);
        });

        builder.Entity<VendorItem>(entity =>
        {
            entity.HasOne(d => d.Item)
                .WithMany(p => p.VendorItem)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VendorItem_Item");

            entity.HasOne(d => d.Vendor)
                .WithMany(p => p.VendorItem)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VendorItem_Vendor");
        });

        builder.Entity<Warehouse>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        builder.Entity<WarehouseItem>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();

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

        builder.Entity<WarehousePickList>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Priority).HasDefaultValue((byte)1);

            entity.HasOne(d => d.AssignedToEmployee)
                .WithMany(p => p.WarehousePickListAssignedToEmployee)
                .HasConstraintName("FK_WarehousePickList_AssignedToEmployee");

            entity.HasOne(d => d.CreatedByEmployee)
                .WithMany(p => p.WarehousePickListCreatedByEmployee)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WarehousePickList_CreatedByEmployee");

            entity.HasOne(d => d.Order).WithMany(p => p.WarehousePickList)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WarehousePickList_Order");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.WarehousePickList)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WarehousePickList_Warehouse");
        });

        builder.Entity<WarehousePickListItem>(entity =>
        {
            entity.HasOne(d => d.PickList).WithMany(p => p.WarehousePickListItem).HasConstraintName("FK_WarehousePickListItem_PickList");

            entity.HasOne(d => d.WarehouseItem).WithMany(p => p.WarehousePickListItem)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WarehousePickListItem_WarehouseItem");

            entity.HasOne(d => d.OrderWarehouseItem).WithMany(p => p.WarehousePickListItem)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WarehousePickListItem_OrderWarehouseItem");
        });
    }

    public Task<int> SaveChangesAsync() => base.SaveChangesAsync();

}
