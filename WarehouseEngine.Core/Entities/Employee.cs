namespace WarehouseEngine.Core.Entities;

public partial class Employee
{
    public Employee()
    {
        InverseSupervisorEmployee = new HashSet<Employee>();
    }

    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public int PositionId { get; set; }
    public int SupervisorEmployeeId { get; set; }
    public byte[]? SocialSecurityNumberHash { get; set; }
    public string? SocialSecuritySerialNumber { get; set; }

    public virtual Position Position { get; set; } = null!;
    public virtual Employee SupervisorEmployee { get; set; } = null!;
    public virtual ICollection<Employee> InverseSupervisorEmployee { get; set; }
}
