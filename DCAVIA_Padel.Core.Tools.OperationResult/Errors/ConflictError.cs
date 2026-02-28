namespace DCAVIA_Padel.Core.Tools.OperationResult.Errors;

public sealed class ConflictError(string message) : ResultError("CONFLICT", message);