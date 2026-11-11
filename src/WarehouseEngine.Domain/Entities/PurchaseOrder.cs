using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WarehouseEngine.Domain.Entities;

public partial class PurchaseOrder
{
    private PurchaseOrder()
    {
        PurchaseOrderWarehouseItem = new HashSet<PurchaseOrderWarehouseItem>();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; private set; }

    [Column(TypeName = "date")]
    public DateTime OrderDate { get; private set; }

    [StringLength(MaxOrderNumberLength)]
    public string OrderNumber { get; private set; } = null!;

    public byte Status { get; private set; }

    [InverseProperty("PurchaseOrder")]
    public virtual ICollection<PurchaseOrderWarehouseItem> PurchaseOrderWarehouseItem { get; private set; }
}
