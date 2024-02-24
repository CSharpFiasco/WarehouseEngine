using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WarehouseEngine.Domain.Entities;

public partial class PurchaseOrder
{
    public PurchaseOrder()
    {
        PurchaseOrderWarehouseItem = new HashSet<PurchaseOrderWarehouseItem>();
    }

    [Key]
    public Guid Id { get; set; }
    [Column(TypeName = "date")]
    public DateTime OrderDate { get; set; }
    [StringLength(255)]
    [Unicode(false)]
    public required string OrderNumber { get; set; }
    public byte Status { get; set; }

    [InverseProperty("PurchaseOrder")]
    public virtual ICollection<PurchaseOrderWarehouseItem> PurchaseOrderWarehouseItem { get; init; }
}
