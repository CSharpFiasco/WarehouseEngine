using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WarehouseEngine.Domain.Entities;

[Index("PositionId", Name = "IX_Employee01")]
[Index("SupervisorEmployeeId", Name = "IX_Employee02")]
public partial class Employee
{
    private Employee()
    {
        InverseSupervisorEmployee = new HashSet<Employee>();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; private set; }

    [StringLength(MaxNameLength)]
    public string FirstName { get; private set; } = null!;

    [StringLength(MaxNameLength)]
    public string? MiddleName { get; private set; }

    [StringLength(MaxNameLength)]
    public string LastName { get; private set; } = null!;

    [StringLength(MaxUserNameLength)]
    public string UserName { get; private set; } = null!;

    public Guid PositionId { get; private set; }

    public Guid SupervisorEmployeeId { get; private set; }

    private byte[]? _socialSecurityNumberHash;

    [MaxLength(MaxHashLength)]
    public byte[]? SocialSecurityNumberHash
    {
        get => _socialSecurityNumberHash?.ToArray();
        private set => _socialSecurityNumberHash = value;
    }

    [StringLength(MaxSerialNumberLength)]
    public string? SocialSecuritySerialNumber { get; private set; }

    [ForeignKey("PositionId")]
    [InverseProperty("Employee")]
    public virtual Position? Position { get; private set; }

    [ForeignKey("SupervisorEmployeeId")]
    [InverseProperty("InverseSupervisorEmployee")]
    public virtual Employee SupervisorEmployee { get; private set; } = null!;

    [InverseProperty("SupervisorEmployee")]
    public virtual ICollection<Employee> InverseSupervisorEmployee { get; private set; }
}
