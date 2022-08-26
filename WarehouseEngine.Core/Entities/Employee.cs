namespace WarehouseEngine.Core.Entities;

public partial class Employee
{
    public Employee()
    {
        InverseSupervisorEmployee = new HashSet<Employee>();
    }

    public int Id { get; set; }
    public required string FirstName { get; set; }
    public string? MiddleName { get; set; }
    public required string LastName { get; set; }
    public required string UserName { get; set; }
    public int PositionId { get; set; }
    public int SupervisorEmployeeId { get; set; }
    public byte[]? SocialSecurityNumberHash { get; set; }
    public string? SocialSecuritySerialNumber { get; set; }

    public virtual Position? Position { get; set; }
    public virtual Employee? SupervisorEmployee { get; set; }
    public virtual ICollection<Employee> InverseSupervisorEmployee { get; set; }
}
