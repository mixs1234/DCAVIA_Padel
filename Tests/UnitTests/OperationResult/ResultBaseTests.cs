using DCAVIA_Padel.Core.Tools.OperationResult;

namespace UnitTests.OperationResult;

/// <summary>
/// Tests for ResultBase static factory methods and combining behavior.
/// </summary>
public class ResultBaseTests
{
    #region Requirement 1: Static Factory Methods

    [Fact]
    public void Ok_CreatesSuccessfulResult()
    {
        // Act
        var result = ResultBase.Ok();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Null(result.Error);
    }

    [Fact]
    public void Ok_WithValue_CreatesSuccessfulResultWithPayload()
    {
        // Arrange
        var expectedValue = "test payload";

        // Act
        var result = ResultBase.Ok(expectedValue);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal(expectedValue, result.Value);
        Assert.Null(result.Error);
    }

    [Fact]
    public void Fail_WithResultError_CreatesFailedResult()
    {
        // Arrange
        var error = new ResultError("TEST_ERROR", "Test error message");

        // Act
        var result = ResultBase.Fail(error);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(error, result.Error);
    }

    [Fact]
    public void Fail_WithCodeAndMessage_CreatesFailedResult()
    {
        // Arrange
        var errorCode = "TEST_ERROR";
        var message = "Test error message";

        // Act
        var result = ResultBase.Fail(errorCode, message);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(errorCode, result.Error?.ErrorCode);
        Assert.Contains(message, result.Error?.ErrorMessages ?? new List<string>());
    }

    [Fact]
    public void Fail_Generic_WithResultError_CreatesFailedResultWithType()
    {
        // Arrange
        var error = new ResultError("TEST_ERROR", "Test error");

        // Act
        var result = ResultBase.Fail<string>(error);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(error, result.Error);
        Assert.Null(result.Value);
    }

    #endregion

    #region Requirement 3: Combine Multiple Results (Helper Methods)

    [Fact]
    public void AssertAll_AllSuccess_ReturnsSuccess()
    {
        // Arrange
        Func<Result> validation1 = () => ResultBase.Ok();
        Func<Result> validation2 = () => ResultBase.Ok();
        Func<Result> validation3 = () => ResultBase.Ok();

        // Act
        var result = ResultBase.AssertAll(validation1, validation2, validation3);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Null(result.Error);
    }

    [Fact]
    public void AssertAll_SomeFailures_ReturnsFailureWithAllErrors()
    {
        // Arrange
        Func<Result> validation1 = () => ResultBase.Fail("ERR1", "First error");
        Func<Result> validation2 = () => ResultBase.Ok();
        Func<Result> validation3 = () => ResultBase.Fail("ERR2", "Second error");

        // Act
        var result = ResultBase.AssertAll(validation1, validation2, validation3);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
        Assert.Equal(2, result.Error.ErrorMessages.Count);
        Assert.Contains("First error", result.Error.ErrorMessages);
        Assert.Contains("Second error", result.Error.ErrorMessages);
    }

    [Fact]
    public void AssertAll_AllFailures_CollectsAllErrors()
    {
        // Arrange
        Func<Result> validation1 = () => ResultBase.Fail("ERR1", "Error one");
        Func<Result> validation2 = () => ResultBase.Fail("ERR2", "Error two");
        Func<Result> validation3 = () => ResultBase.Fail("ERR3", "Error three");

        // Act
        var result = ResultBase.AssertAll(validation1, validation2, validation3);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(3, result.Error!.ErrorMessages.Count);
    }

    [Fact]
    public void AssertAll_WithCustomErrorCode_UsesThatCode()
    {
        // Arrange
        var customCode = "CUSTOM_VALIDATION_ERROR";
        Func<Result> validation1 = () => ResultBase.Fail("ERR1", "Error");

        // Act
        var result = ResultBase.AssertAll(customCode, validation1);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(customCode, result.Error?.ErrorCode);
    }

    [Fact]
    public void AssertAll_WithoutErrorCode_UsesDefaultCode()
    {
        // Arrange
        Func<Result> validation1 = () => ResultBase.Fail("ERR1", "Error");

        // Act
        var result = ResultBase.AssertAll(validation1);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("VALIDATION_ERROR", result.Error?.ErrorCode);
    }

    [Fact]
    public void Combine_AllSuccess_ReturnsSuccess()
    {
        // Arrange
        var result1 = ResultBase.Ok();
        var result2 = ResultBase.Ok<string>("test");
        var result3 = ResultBase.Ok();

        // Act
        var combined = ResultBase.Combine(result1, result2, result3);

        // Assert
        Assert.True(combined.IsSuccess);
    }

    [Fact]
    public void Combine_SomeFailures_CollectsAllErrors()
    {
        // Arrange
        var result1 = ResultBase.Fail("ERR1", "First");
        var result2 = ResultBase.Ok();
        var result3 = ResultBase.Fail("ERR2", "Second");

        // Act
        var combined = ResultBase.Combine(result1, result2, result3);

        // Assert
        Assert.False(combined.IsSuccess);
        Assert.Equal(2, combined.Error!.ErrorMessages.Count);
    }

    #endregion
}
