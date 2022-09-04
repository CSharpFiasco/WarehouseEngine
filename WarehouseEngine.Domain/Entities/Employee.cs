using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WarehouseEngine.Domain.Entities;

[Index("PositionId", Name = "IX_Employee01")]
[Index("SupervisorEmployeeId", Name = "IX_Employee02")]
public partial class Employee
{
    public Employee()
    {
        InverseSupervisorEmployee = new HashSet<Employee>();
    }

    [Key]
    public int Id { get; set; }
    [StringLength(30)]
    [Unicode(false)]
    public required string FirstName { get; set; }
    [StringLength(30)]
    [Unicode(false)]
    public string? MiddleName { get; set; }
    [StringLength(30)]
    [Unicode(false)]
    public required string LastName { get; set; }
    [StringLength(32)]
    [Unicode(false)]
    public required string UserName { get; set; }
    public int PositionId { get; set; }
    public int SupervisorEmployeeId { get; set; }
    [MaxLength(32)]
    public byte[]? SocialSecurityNumberHash { get; set; }
    [StringLength(4)]
    [Unicode(false)]
    public string? SocialSecuritySerialNumber { get; set; }

    [ForeignKey("PositionId")]
    [InverseProperty("Employee")]
    public Position? Position { get; set; }

    [ForeignKey("SupervisorEmployeeId")]
    [InverseProperty("InverseSupervisorEmployee")]
    public virtual required Employee SupervisorEmployee { get; set; }

    [InverseProperty("SupervisorEmployee")]
    public virtual ICollection<Employee> InverseSupervisorEmployee { get; set; }
}
