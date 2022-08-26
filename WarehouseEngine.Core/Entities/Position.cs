namespace WarehouseEngine.Core.Entities;

public partial class Position
{
    public Position()
    {
        Employee = new HashSet<Employee>();
    }

    public int Id { get; set; }
    public required string Name { get; set; }

    public virtual ICollection<Employee> Employee { get; set; }
}
