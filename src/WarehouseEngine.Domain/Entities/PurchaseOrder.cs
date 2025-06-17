using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WarehouseEngine.Domain.Entities;

public partial class PurchaseOrder
{
    [Key]
    public required Guid Id { get; set; }

    [Column(TypeName = "date")]
    public DateTime OrderDate { get; set; }

    [StringLength(255)]
    public required string OrderNumber { get; set; }

    public byte Status { get; set; }

    public DateTime DateCreated { get; set; }

    [StringLength(80)]
    public string CreatedBy { get; set; } = null!;

    public DateTime? DateModified { get; set; }

    [StringLength(80)]
    public string? ModifiedBy { get; set; }

    [InverseProperty("PurchaseOrder")]
    public virtual ICollection<PurchaseOrderWarehouseItem> PurchaseOrderWarehouseItem { get; init; } = new List<PurchaseOrderWarehouseItem>();
}
