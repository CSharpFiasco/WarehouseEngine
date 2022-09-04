using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace WarehouseEngine.Domain.Entities;

public partial class Company
{
    [Key]
    public int Id { get; set; }
    [StringLength(80)]
    [Unicode(false)]
    public required string Name { get; set; }
}
