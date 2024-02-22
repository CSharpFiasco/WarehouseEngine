using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WarehouseEngine.Domain.Entities;

namespace WarehouseEngine.Infrastructure.DataContext;

[ExcludeFromCodeCoverage]
public partial class WarehouseEngineContext : IdentityDbContext<IdentityUser>
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
    }

    public virtual DbSet<Customer> Customer { get; set; } = null!;
    public virtual DbSet<Order> Order { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);


        builder.Entity<Customer>(entity =>
        {
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

            entity.Property(e => e.DateCreated).HasDefaultValueSql("getutcdate()");
        });

        builder.Entity<Order>(entity =>
        {
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
    }

    public Task<int> SaveChangesAsync() => base.SaveChangesAsync();
}
