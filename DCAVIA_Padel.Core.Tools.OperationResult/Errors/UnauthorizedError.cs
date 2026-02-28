namespace DCAVIA_Padel.Core.Tools.OperationResult.Errors;

public sealed class UnauthorizedError(string message = "You are not authorized to perform this action.")
    : ResultError("UNAUTHORIZED", message);