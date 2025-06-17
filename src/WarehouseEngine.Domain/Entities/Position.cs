using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WarehouseEngine.Domain.Entities;

public partial class Position
{
    [Key]
    public required Guid Id { get; set; }

    [StringLength(80)]
    public required string Name { get; set; }

    [InverseProperty("Position")]
    public virtual ICollection<Employee> Employee { get; init; } = new List<Employee>();
}
