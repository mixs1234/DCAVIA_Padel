namespace DCAVIA_Padel.Core.Tools.OperationResult;

public abstract class ResultError(string errorCode, string errorMessage)
{
    public string ErrorCode { get; } = errorCode;
    public string Message { get; } = errorMessage;

    public override string ToString() => $"[{ErrorCode}] {Message}";
}