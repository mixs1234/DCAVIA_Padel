using DCAVIA_Padel.Core.Tools.OperationResult;
using DCAVIA_Padel.Core.Tools.OperationResult.Errors;

namespace UnitTests.OperationResult;

public class ResultGenericTests
{
    #region Basic Functionality

    [Fact]
    public void Success_StoresValue()
    {
        // Arrange
        var value = 42;

        // Act
        var result = ResultBase.Ok(value);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(value, result.Value);
        Assert.Null(result.Error);
    }

    [Fact]
    public void Failure_StoresError()
    {
        // Arrange
        var error = new ConflictError("Error");

        // Act
        var result = ResultBase.Fail<int>(error);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(error, result.Error);
        Assert.Equal(0, result.Value); // default(int)
    }

    #endregion

    #region WithoutValue

    [Fact]
    public void WithoutValue_Success_ReturnsSuccessResultWithoutPayload()
    {
        // Arrange
        var result = ResultBase.Ok("value");

        // Act
        var withoutValue = result.WithoutValue();

        // Assert
        Assert.True(withoutValue.IsSuccess);
        Assert.Null(withoutValue.Error);
    }

    [Fact]
    public void WithoutValue_Failure_ForwardsError()
    {
        // Arrange
        var error = new ConflictError("Error");
        var result = ResultBase.Fail<string>(error);

        // Act
        var withoutValue = result.WithoutValue();

        // Assert
        Assert.False(withoutValue.IsSuccess);
        Assert.Equal(error, withoutValue.Error);
    }

    #endregion

    #region Implicit Operators

    [Fact]
    public void ImplicitOperator_FromValue_CreatesSuccess()
    {
        // Act
        Result<int> result = 42; // Implicit conversion from int

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void ImplicitOperator_FromResultError_CreatesFailure()
    {
        // Arrange
        var error = new ConflictError("Test");

        // Act
        Result<string> result = error; // Implicit conversion

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(error, result.Error);
    }

    [Fact]
    public void ImplicitOperator_AllowsReturningValueDirectly()
    {
        // Act
        var result = GetUser(1);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("User_1", result.Value);
    }

    [Fact]
    public void ImplicitOperator_AllowsReturningErrorDirectly()
    {
        // Act
        var result = GetUser(999);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("NOT_FOUND", result.Error?.ErrorCode);
    }

    // Simulates a method benefiting from implicit operators
    private Result<string> GetUser(int id)
    {
        if (id == 999)
            return new NotFoundError("User", id); // implicit

        return $"User_{id}"; // implicit
    }

    #endregion

    #region Railway Oriented Programming - Then

    [Fact]
    public void Then_Success_CallsNextFunction()
    {
        // Arrange
        var result = ResultBase.Ok(5);

        // Act
        var doubled = result.Then(x => ResultBase.Ok(x * 2));

        // Assert
        Assert.True(doubled.IsSuccess);
        Assert.Equal(10, doubled.Value);
    }

    [Fact]
    public void Then_Failure_DoesNotCallNext()
    {
        // Arrange
        var error = new ConflictError("Error");
        var result = ResultBase.Fail<int>(error);
        var wasCalled = false;

        // Act
        var chained = result.Then(x =>
        {
            wasCalled = true;
            return ResultBase.Ok(x * 2);
        });

        // Assert
        Assert.False(chained.IsSuccess);
        Assert.False(wasCalled);
        Assert.Equal(error, chained.Error);
    }

    [Fact]
    public void Then_Chained_PropagatesFirstError()
    {
        // Arrange
        var result = ResultBase.Ok(10);

        // Act
        var chained = result
            .Then(x => ResultBase.Ok(x * 2))
            .Then(x => ResultBase.Fail<int>(new ConflictError("Broke here")))
            .Then(x => ResultBase.Ok(x + 100)); // should not execute

        // Assert
        Assert.False(chained.IsSuccess);
        Assert.Contains("Broke here", chained.Error?.Message);
    }

    #endregion

    #region Railway Oriented Programming - Map

    [Fact]
    public void Map_Success_TransformsValue()
    {
        // Arrange
        var result = ResultBase.Ok(5);

        // Act
        var mapped = result.Map(x => x.ToString());

        // Assert
        Assert.True(mapped.IsSuccess);
        Assert.Equal("5", mapped.Value);
    }

    [Fact]
    public void Map_Failure_ForwardsError()
    {
        // Arrange
        var error = new ConflictError("Error");
        var result = ResultBase.Fail<int>(error);

        // Act
        var mapped = result.Map(x => x.ToString());

        // Assert
        Assert.False(mapped.IsSuccess);
        Assert.Equal(error, mapped.Error);
    }

    #endregion

    #region Railway Oriented Programming - OnSuccess / OnFailure

    [Fact]
    public void OnSuccess_Success_ExecutesAction()
    {
        // Arrange
        var capturedValue = 0;
        var result = ResultBase.Ok(42);

        // Act
        var returned = result.OnSuccess(v => capturedValue = v);

        // Assert
        Assert.Equal(42, capturedValue);
        Assert.Same(result, returned);
    }

    [Fact]
    public void OnSuccess_Failure_DoesNotExecute()
    {
        // Arrange
        var executed = false;
        var result = ResultBase.Fail<int>(new ConflictError("Error"));

        // Act
        result.OnSuccess(v => executed = true);

        // Assert
        Assert.False(executed);
    }

    [Fact]
    public void OnFailure_Failure_ExecutesAction()
    {
        // Arrange
        ResultError? capturedError = null;
        var error = new ConflictError("Error");
        var result = ResultBase.Fail<int>(error);

        // Act
        var returned = result.OnFailure(e => capturedError = e);

        // Assert
        Assert.Equal(error, capturedError);
        Assert.Same(result, returned);
    }

    [Fact]
    public void OnFailure_Success_DoesNotExecute()
    {
        // Arrange
        var executed = false;
        var result = ResultBase.Ok(42);

        // Act
        result.OnFailure(e => executed = true);

        // Assert
        Assert.False(executed);
    }

    #endregion

    #region Railway Oriented Programming - Match

    [Fact]
    public void Match_Success_CallsOnSuccessBranch()
    {
        // Arrange
        var result = ResultBase.Ok(10);

        // Act
        var output = result.Match(
            onSuccess: x => $"Success: {x}",
            onFailure: e => $"Failure: {e.ErrorCode}"
        );

        // Assert
        Assert.Equal("Success: 10", output);
    }

    [Fact]
    public void Match_Failure_CallsOnFailureBranch()
    {
        // Arrange
        var error = new ConflictError("Error");
        var result = ResultBase.Fail<int>(error);

        // Act
        var output = result.Match(
            onSuccess: x => $"Success: {x}",
            onFailure: e => $"Failure: {e.ErrorCode}"
        );

        // Assert
        Assert.Equal("Failure: CONFLICT", output);
    }

    #endregion
}