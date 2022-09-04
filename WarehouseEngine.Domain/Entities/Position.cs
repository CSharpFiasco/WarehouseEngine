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
    public int Id { get; set; }

    [StringLength(80)]
    [Unicode(false)]
    public required string Name { get; set; }

    [InverseProperty("Position")]
    public virtual ICollection<Employee> Employee { get; set; }
}
