namespace DCAVIA_Padel.Core.Tools.OperationResult;

public class ResultError
{
    public string ErrorCode { get; }
    public List<string> ErrorMessages { get; }
    public string? StackTrace { get; }
    
    public ResultError(string errorCode, string errorMessage, string? stackTrace = null)
        : this(errorCode, new List<string> { errorMessage }, stackTrace)
    {
    }
    
    public ResultError(string errorCode, List<string> errorMessages, string? stackTrace = null)
    {
        ErrorCode = errorCode;
        ErrorMessages = errorMessages;
        StackTrace = stackTrace;
    }
    
    public static ResultError Merge(string errorCode, IEnumerable<ResultError> errors)
    {
        var messages = errors.SelectMany(e => e.ErrorMessages).ToList();
        return new ResultError(errorCode, messages);
    }

    public override string ToString() =>
        $"ErrorCode: {ErrorCode}, Messages: [{string.Join(", ", ErrorMessages)}]";
}