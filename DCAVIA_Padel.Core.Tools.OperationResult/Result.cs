namespace DCAVIA_Padel.Core.Tools.OperationResult;

public class Result : ResultBase
{
    internal Result(bool isSuccess, ResultError? error) : base(isSuccess, error)
    {
    }
}

public class Result<T> : ResultBase
{
    public T Value { get; }
    
    public Result(bool isSuccess, ResultError? error) : base(isSuccess, error)
    {
    }
}