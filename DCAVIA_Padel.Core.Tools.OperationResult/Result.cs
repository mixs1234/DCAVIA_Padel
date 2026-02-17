namespace DCAVIA_Padel.Core.Tools.OperationResult;

public class Result : ResultBase
{
    internal Result(bool isSuccess, ResultError? error) : base(isSuccess, error) { }
    
    public Result<T> WithValue<T>(T value) =>
        IsSuccess
            ? ResultBase.Ok(value)
            : ResultBase.Fail<T>(Error!);
    
    // Allows returning a ResultError directly where a Result is expected
    public static implicit operator Result(ResultError error) =>
        ResultBase.Fail(error);
}

public class Result<T> : ResultBase
{
    public T? Value { get; }

    internal Result(T value) : base(true, null) => Value = value;
    internal Result(ResultError error) : base(false, error) { }
    
    public Result WithoutValue() =>
        IsSuccess
            ? ResultBase.Ok()
            : ResultBase.Fail(Error!);
    
    /// <summary>
    /// If successful, passes the value into the next operation.
    /// If already failed, the error is forwarded without calling next.
    /// </summary>
    public Result<TNext> Then<TNext>(Func<T, Result<TNext>> next) =>
        IsSuccess ? next(Value!) : ResultBase.Fail<TNext>(Error!);

    public Result Then(Func<T, Result> next) =>
        IsSuccess ? next(Value!) : ResultBase.Fail(Error!);

    /// <summary>
    /// Transform the value if successful, similar to LINQ Select.
    /// </summary>
    public Result<TNext> Map<TNext>(Func<T, TNext> mapper) =>
        IsSuccess
            ? ResultBase.Ok(mapper(Value!))
            : ResultBase.Fail<TNext>(Error!);
    
    /// <summary>
    /// Execute an action on the value if successful, then return the same result.
    /// Useful for side effects (e.g. logging) without breaking the chain.
    /// </summary>
    public Result<T> OnSuccess(Action<T> action)
    {
        if (IsSuccess) action(Value!);
        return this;
    }
    
    /// <summary>
    /// Execute an action on the error if failed, then return the same result.
    /// Useful for logging or observing failures without handling them.
    /// </summary>
    public Result<T> OnFailure(Action<ResultError> action)
    {
        if (IsFailure) action(Error!);
        return this;
    }
    
    /// <summary>
    /// Pattern match on success or failure.
    /// </summary>
    public TOut Match<TOut>(Func<T, TOut> onSuccess, Func<ResultError, TOut> onFailure) =>
        IsSuccess ? onSuccess(Value!) : onFailure(Error!);
    
    // Allows returning a value or error directly without wrapping manually:
    //   return myObject;               // instead of ResultBase.Ok(myObject)
    //   return PadelErrors.NotFound;   // instead of ResultBase.Fail<T>(PadelErrors.NotFound)
    public static implicit operator Result<T>(T value) =>
        ResultBase.Ok(value);
    public static implicit operator Result<T>(ResultError error) =>
        ResultBase.Fail<T>(error);
}