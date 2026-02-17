namespace DCAVIA_Padel.Core.Tools.OperationResult;

public abstract class ResultBase
{
    public bool IsSuccess { get; set; }
    public ResultError? Error { get; set; }
    
    protected  ResultBase(bool isSuccess, ResultError? error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }
    
    public static Result Ok() => new(true, null);
    
}