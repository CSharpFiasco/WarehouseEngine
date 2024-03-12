using Microsoft.EntityFrameworkCore.ValueGeneration;
using UUIDNext;
using WarehouseEngine.Application.Interfaces;

namespace WarehouseEngine.Application.Implementations;
public class SequentialIdGenerator : IIdGenerator
{
    private readonly SequentialGuidValueGenerator _sequentialGuidValueGenerator;

    public SequentialIdGenerator() {
        _sequentialGuidValueGenerator = new();
    }

    /// <summary>
    /// Generate a new sequential GUID. SequentialGuidValueGenerator models the SQL Server NEWSEQUENTIALID() function.
    /// </summary>
    /// <returns></returns>
    public Guid NewId() => _sequentialGuidValueGenerator.Next(default!);
}
