using DCAVIA_Padel.Core.Tools.OperationResult;

namespace UnitTests.OperationResult;

/// <summary>
/// Tests for Result (non-generic) class.
/// </summary>
public class ResultTests
{
    [Fact]
    public void WithValue_Success_ReturnsResultWithPayload()
    {
        // Arrange
        var result = ResultBase.Ok();
        var value = "test value";

        // Act
        var withValue = result.WithValue(value);

        // Assert
        Assert.True(withValue.IsSuccess);
        Assert.Equal(value, withValue.Value);
    }

    [Fact]
    public void WithValue_Failure_ForwardsError()
    {
        // Arrange
        var error = new ResultError("ERR", "Error message");
        var result = ResultBase.Fail(error);

        // Act
        var withValue = result.WithValue("some value");

        // Assert
        Assert.False(withValue.IsSuccess);
        Assert.Equal(error, withValue.Error);
        Assert.Null(withValue.Value);
    }

    #region Requirement 4: Implicit Operators

    [Fact]
    public void ImplicitOperator_FromResultError_CreatesFailure()
    {
        // Arrange
        var error = new ResultError("TEST", "Test error");

        // Act
        Result result = error; // Implicit conversion

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(error, result.Error);
    }

    [Fact]
    public void ImplicitOperator_AllowsReturningErrorDirectly()
    {
        // Act
        var result = MethodThatReturnsResultError();

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("DIRECT_ERROR", result.Error?.ErrorCode);
    }

    private Result MethodThatReturnsResultError()
    {
        // This tests the implicit conversion - we can return ResultError where Result is expected
        return new ResultError("DIRECT_ERROR", "Returned directly");
    }

    #endregion
}