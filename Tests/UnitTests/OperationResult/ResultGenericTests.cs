using DCAVIA_Padel.Core.Tools.OperationResult;

namespace UnitTests.OperationResult;


/// <summary>
/// Tests for Result generic class
/// </summary>
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
        var error = new ResultError("ERR", "Error");

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
        var error = new ResultError("ERR", "Error");
        var result = ResultBase.Fail<string>(error);

        // Act
        var withoutValue = result.WithoutValue();

        // Assert
        Assert.False(withoutValue.IsSuccess);
        Assert.Equal(error, withoutValue.Error);
    }

    #endregion

    #region Requirement 4: Implicit Operators

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
        var error = new ResultError("TEST", "Test");

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
        Assert.Equal("USER_NOT_FOUND", result.Error?.ErrorCode);
    }

    // Simulates a method benefiting from implicit operators
    private Result<string> GetUser(int id)
    {
        if (id == 999)
            return new ResultError("USER_NOT_FOUND", "User does not exist"); // implicit

        return $"User_{id}"; // implicit
    }

    #endregion

    #region Requirement 5: Railway Oriented Programming - Then

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
        var error = new ResultError("ERR", "Error");
        var result = ResultBase.Fail<int>(error);
        var nextCalled = false;

        // Act
        var chained = result.Then(x =>
        {
            nextCalled = true;
            return ResultBase.Ok(x * 2);
        });

        // Assert
        Assert.False(nextCalled);
        Assert.False(chained.IsSuccess);
        Assert.Equal(error, chained.Error);
    }

    [Fact]
    public void Then_ChainMultipleOperations_ShortCircuitsOnFirstFailure()
    {
        // Arrange
        var step2Called = false;
        var step3Called = false;

        // Act
        var result = ResultBase.Ok(10)
            .Then(x => ResultBase.Fail<int>(new ResultError("ERR", "Failed at step 1")))
            .Then(x =>
            {
                step2Called = true;
                return ResultBase.Ok(x * 2);
            })
            .Then(x =>
            {
                step3Called = true;
                return ResultBase.Ok(x + 5);
            });

        // Assert
        Assert.False(result.IsSuccess);
        Assert.False(step2Called, "Step 2 should not execute after failure");
        Assert.False(step3Called, "Step 3 should not execute after failure");
    }

    [Fact]
    public void Then_NonGeneric_Success_Executes()
    {
        // Arrange
        var result = ResultBase.Ok("input");
        var executed = false;

        // Act
        var chained = result.Then(x =>
        {
            executed = true;
            return ResultBase.Ok();
        });

        // Assert
        Assert.True(executed);
        Assert.True(chained.IsSuccess);
    }

    #endregion

    #region Requirement 5: Railway Oriented Programming - Map

    [Fact]
    public void Map_Success_TransformsValue()
    {
        // Arrange
        var result = ResultBase.Ok(5);

        // Act
        var squared = result.Map(x => x * x);

        // Assert
        Assert.True(squared.IsSuccess);
        Assert.Equal(25, squared.Value);
    }

    [Fact]
    public void Map_Failure_DoesNotTransform()
    {
        // Arrange
        var error = new ResultError("ERR", "Error");
        var result = ResultBase.Fail<int>(error);
        var mapCalled = false;

        // Act
        var mapped = result.Map(x =>
        {
            mapCalled = true;
            return x * 2;
        });

        // Assert
        Assert.False(mapCalled);
        Assert.False(mapped.IsSuccess);
        Assert.Equal(error, mapped.Error);
    }

    [Fact]
    public void Map_ChainTransformations_WorksLikeLinq()
    {
        // Act
        var result = ResultBase.Ok("hello")
            .Map(s => s.ToUpper())
            .Map(s => s + " WORLD")
            .Map(s => s.Length);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(11, result.Value);
    }

    #endregion

    #region Requirement 5: Railway Oriented Programming - OnSuccess / OnFailure

    [Fact]
    public void OnSuccess_Success_ExecutesAction()
    {
        // Arrange
        var sideEffectValue = 0;
        var result = ResultBase.Ok(42);

        // Act
        var returned = result.OnSuccess(x => sideEffectValue = x);

        // Assert
        Assert.Equal(42, sideEffectValue);
        Assert.Same(result, returned); // Should return same result
    }

    [Fact]
    public void OnSuccess_Failure_DoesNotExecute()
    {
        // Arrange
        var executed = false;
        var result = ResultBase.Fail<int>(new ResultError("ERR", "Error"));

        // Act
        result.OnSuccess(x => executed = true);

        // Assert
        Assert.False(executed);
    }

    [Fact]
    public void OnFailure_Failure_ExecutesAction()
    {
        // Arrange
        ResultError? capturedError = null;
        var error = new ResultError("ERR", "Error");
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

    #region Requirement 5: Railway Oriented Programming - Match

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
        var error = new ResultError("ERR", "Error");
        var result = ResultBase.Fail<int>(error);

        // Act
        var output = result.Match(
            onSuccess: x => $"Success: {x}",
            onFailure: e => $"Failure: {e.ErrorCode}"
        );

        // Assert
        Assert.Equal("Failure: ERR", output);
    }

    #endregion
}