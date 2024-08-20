namespace WarehouseEngine.Domain.ErrorTypes;
public record EntityErrorType();

public record EntityAlreadyExists : EntityErrorType { };

public record EntityDoesNotExist : EntityErrorType { };
