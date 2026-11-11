using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WarehouseEngine.Domain.Entities;

public partial class Position
{
    private Position()
    {
        Employee = new HashSet<Employee>();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; private set; }

    [StringLength(MaxNameLength)]
    public string Name { get; private set; } = null!;

    [InverseProperty("Position")]
    public virtual ICollection<Employee> Employee { get; private set; }
}
