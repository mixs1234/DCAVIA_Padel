namespace DCAVIA_Padel.Core.Tools.OperationResult;

public abstract class ResultBase
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public ResultError? Error { get; }

    protected ResultBase(bool isSuccess, ResultError? error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }
    
    public static Result Ok() =>
        new(true, null);

    public static Result<T> Ok<T>(T value) =>
        new(value);

    public static Result Fail(ResultError error) =>
        new(false, error);

    public static Result Fail(string errorCode, List<string> message) =>
        Fail(new ResultError(errorCode, message));

    public static Result<T> Fail<T>(ResultError error) =>
        new(error);

    public static Result<T> Fail<T>(string errorCode, List<string> message) =>
        Fail<T>(new ResultError(errorCode, message));
    
    public static Result AssertAll(string mergedErrorCode, params Func<Result>[] assertions)
    {
        var errors = assertions
            .Select(assertion => assertion())
            .Where(result => result.IsFailure)
            .Select(result => result.Error!)
            .ToList();

        return errors.Count == 0
            ? Ok()
            : Fail(ResultError.Merge(mergedErrorCode, errors));
    }
    
    /// <summary>
    /// AssertAll with default error code "VALIDATION_ERROR".
    /// </summary>
    public static Result AssertAll(params Func<Result>[] assertions) =>
        AssertAll("VALIDATION_ERROR", assertions);
    
    
    public static Result Combine(params ResultBase[] results)
    {
        var errors = results
            .Where(r => r.IsFailure)
            .Select(r => r.Error!)
            .ToList();

        return errors.Count == 0
            ? Ok()
            : Fail(ResultError.Merge("COMBINED_ERROR", errors));
    }
}