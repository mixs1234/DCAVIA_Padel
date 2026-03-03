using DCAVIA_Padel.Core.Tools.OperationResult.Errors;

namespace DCAVIA_Padel.Core.Tools.OperationResult;

public class Result : ResultBase
{
    internal Result(bool isSuccess, ResultError? error) : base(isSuccess, error) { }
    
    public Result<T> WithValue<T>(T value) =>
        IsSuccess
            ? Ok(value)
            : Fail<T>(Error!);
    
    public static implicit operator Result(ResultError error) =>
        Fail(error);
}

public class Result<T> : ResultBase
{
    public T? Value { get; }

    internal Result(T value) : base(true, null) => Value = value;
    internal Result(ResultError error) : base(false, error) { }
    
    public Result WithoutValue() =>
        IsSuccess
            ? Ok()
            : Fail(Error!);
    
    public Result<TNext> Then<TNext>(Func<T, Result<TNext>> next) =>
        IsSuccess ? next(Value!) : Fail<TNext>(Error!);

    public Result Then(Func<T, Result> next) =>
        IsSuccess ? next(Value!) : Fail(Error!);
    
    public Result<TNext> Map<TNext>(Func<T, TNext> mapper) =>
        IsSuccess
            ? Ok(mapper(Value!))
            : Fail<TNext>(Error!);
    
    public Result<T> OnSuccess(Action<T> action)
    {
        if (IsSuccess) action(Value!);
        return this;
    }
    
    public Result<T> OnFailure(Action<ResultError> action)
    {
        if (IsFailure) action(Error!);
        return this;
    }

    public TOut Match<TOut>(Func<T, TOut> onSuccess, Func<ResultError, TOut> onFailure) =>
        IsSuccess ? onSuccess(Value!) : onFailure(Error!);
    
    // Auto wrap values and errors
    public static implicit operator Result<T>(T value) =>
        Ok(value);
    public static implicit operator Result<T>(ResultError error) =>
        Fail<T>(error);
}