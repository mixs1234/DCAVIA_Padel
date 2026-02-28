namespace DCAVIA_Padel.Core.Tools.OperationResult.Errors;

public class ConflictError(string message) : ResultError("CONFLICT", message);