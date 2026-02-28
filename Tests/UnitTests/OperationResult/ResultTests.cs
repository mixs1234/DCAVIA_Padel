using DCAVIA_Padel.Core.Tools.OperationResult;
using DCAVIA_Padel.Core.Tools.OperationResult.Errors;

namespace UnitTests.OperationResult;

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
        var error = new ConflictError("Error message");
        var result = ResultBase.Fail(error);

        // Act
        var withValue = result.WithValue("some value");

        // Assert
        Assert.False(withValue.IsSuccess);
        Assert.Equal(error, withValue.Error);
        Assert.Null(withValue.Value);
    }

    #region Implicit Operators

    [Fact]
    public void ImplicitOperator_FromResultError_CreatesFailure()
    {
        // Arrange
        var error = new ConflictError("Test error");

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
        Assert.Equal("CONFLICT", result.Error?.ErrorCode);
    }

    private Result MethodThatReturnsResultError()
    {
        return new ConflictError("Returned directly");
    }

    #endregion
}