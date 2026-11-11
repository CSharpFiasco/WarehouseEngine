using System.ComponentModel.DataAnnotations;
using WarehouseEngine.Domain.Entities;

namespace WarehouseEngine.Application.Commands;

public sealed class CreateWarehouseCommand
{
    [Required]
    [StringLength(Warehouse.MaxNameLength)]
    public required string Name { get; init; }
}
