namespace DCAVIA_Padel.Core.Tools.OperationResult.Errors;

public class ValidationError(IEnumerable<ValidationDetail> details)
    : ResultError("VALIDATION_ERROR", "One or more validation errors occurred.")
{
    public IReadOnlyList<ValidationDetail> Details { get; } = details.ToList().AsReadOnly();

    public ValidationError(string field, string message)
        : this([new ValidationDetail(field, message)]) { }
}

public record ValidationDetail(string Field, string Message);