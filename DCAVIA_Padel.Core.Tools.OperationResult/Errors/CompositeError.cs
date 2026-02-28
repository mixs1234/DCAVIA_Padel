namespace DCAVIA_Padel.Core.Tools.OperationResult.Errors;

public class CompositeError(IEnumerable<ResultError> errors)
    : ResultError("COMPOSITE_ERROR", "Multiple errors occurred.")
{
    public IReadOnlyList<ResultError> InnerErrors { get; } = errors.ToList().AsReadOnly();
}