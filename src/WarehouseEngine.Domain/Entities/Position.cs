using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WarehouseEngine.Domain.Entities;

public partial class Position
{
    public Position()
    {
        Employee = new HashSet<Employee>();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public required Guid Id { get; set; }

    [StringLength(80)]
    public required string Name { get; set; }

    [InverseProperty("Position")]
    public virtual ICollection<Employee> Employee { get; init; }
}
