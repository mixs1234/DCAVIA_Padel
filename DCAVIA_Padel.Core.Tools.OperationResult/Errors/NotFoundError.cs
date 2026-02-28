namespace DCAVIA_Padel.Core.Tools.OperationResult.Errors;

public class NotFoundError(string entityName, object id)
    : ResultError("NOT_FOUND", $"{entityName} with id '{id}' was not found.")
{
    public string EntityName { get; } = entityName;
    public object Id { get; } = id;
}