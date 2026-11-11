namespace WarehouseEngine.Domain.Entities;

public partial class Employee
{
    public const int MaxNameLength = 30;
    public const int MaxUserNameLength = 32;
    public const int MaxHashLength = 32;
    public const int MaxSerialNumberLength = 4;

    public static Employee Create(Guid id, string firstName, string lastName, string userName,
        Guid positionId, Guid supervisorEmployeeId, string? middleName = null,
        byte[]? socialSecurityNumberHash = null, string? socialSecuritySerialNumber = null)
    {
        EntityRules.Identity(id, nameof(id));
        EntityRules.Identity(positionId, nameof(positionId));
        EntityRules.Identity(supervisorEmployeeId, nameof(supervisorEmployeeId));
        var employee = new Employee
        {
            Id = id,
            PositionId = positionId,
            SupervisorEmployeeId = supervisorEmployeeId
        };
        employee.UpdatePersonalDetails(firstName, middleName, lastName, userName);
        employee.UpdateSocialSecurityDetails(socialSecurityNumberHash, socialSecuritySerialNumber);
        return employee;
    }

    public void UpdatePersonalDetails(string firstName, string? middleName, string lastName, string userName)
    {
        EntityRules.RequiredText(firstName, MaxNameLength, nameof(firstName));
        EntityRules.OptionalText(middleName, MaxNameLength, nameof(middleName));
        EntityRules.RequiredText(lastName, MaxNameLength, nameof(lastName));
        EntityRules.RequiredText(userName, MaxUserNameLength, nameof(userName));
        FirstName = firstName;
        MiddleName = middleName;
        LastName = lastName;
        UserName = userName;
    }

    public void UpdateSocialSecurityDetails(byte[]? hash, string? serialNumber)
    {
        if (hash?.Length > MaxHashLength)
            throw new ArgumentOutOfRangeException(nameof(hash), "Hash must not exceed 32 bytes.");
        EntityRules.OptionalText(serialNumber, MaxSerialNumberLength, nameof(serialNumber));
        SocialSecurityNumberHash = hash?.ToArray();
        SocialSecuritySerialNumber = serialNumber;
    }

    public void AssignPosition(Guid positionId)
    {
        EntityRules.Identity(positionId, nameof(positionId));
        PositionId = positionId;
        Position = null;
    }

    public void AssignSupervisor(Guid supervisorEmployeeId)
    {
        EntityRules.Identity(supervisorEmployeeId, nameof(supervisorEmployeeId));
        SupervisorEmployeeId = supervisorEmployeeId;
        SupervisorEmployee = null!;
    }
}
