using DCAVIA_Padel.Core.Tools.OperationResult;
using DCAVIA_Padel.Core.Tools.OperationResult.Errors;

namespace UnitTests.OperationResult;

public class ResultBaseTests
{
    #region Static Factory Methods

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
        var error = new ConflictError("Test error message");

        // Act
        var result = ResultBase.Fail(error);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(error, result.Error);
    }

    [Fact]
    public void Fail_Generic_WithResultError_CreatesFailedResultWithType()
    {
        // Arrange
        var error = new ConflictError("Test error");

        // Act
        var result = ResultBase.Fail<string>(error);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(error, result.Error);
        Assert.Null(result.Value);
    }

    #endregion

    #region AssertAll and Combine

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
    public void AssertAll_SomeFailures_ReturnsCompositeError()
    {
        // Arrange
        Func<Result> validation1 = () => ResultBase.Fail(new ValidationError("Field1", "First error"));
        Func<Result> validation2 = () => ResultBase.Ok();
        Func<Result> validation3 = () => ResultBase.Fail(new ValidationError("Field2", "Second error"));

        // Act
        var result = ResultBase.AssertAll(validation1, validation2, validation3);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
        Assert.IsType<CompositeError>(result.Error);

        var composite = (CompositeError)result.Error;
        Assert.Equal(2, composite.InnerErrors.Count);
    }

    [Fact]
    public void AssertAll_AllFailures_CollectsAllErrors()
    {
        // Arrange
        Func<Result> validation1 = () => ResultBase.Fail(new ConflictError("Error one"));
        Func<Result> validation2 = () => ResultBase.Fail(new ConflictError("Error two"));
        Func<Result> validation3 = () => ResultBase.Fail(new ConflictError("Error three"));

        // Act
        var result = ResultBase.AssertAll(validation1, validation2, validation3);

        // Assert
        Assert.False(result.IsSuccess);
        var composite = Assert.IsType<CompositeError>(result.Error);
        Assert.Equal(3, composite.InnerErrors.Count);
    }

    [Fact]
    public void AssertAll_Failure_HasCompositeErrorCode()
    {
        // Arrange
        Func<Result> validation = () => ResultBase.Fail(new ConflictError("Error"));

        // Act
        var result = ResultBase.AssertAll(validation);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("COMPOSITE_ERROR", result.Error?.ErrorCode);
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
    public void Combine_SomeFailures_ReturnsCompositeError()
    {
        // Arrange
        var result1 = ResultBase.Fail(new ConflictError("First"));
        var result2 = ResultBase.Ok();
        var result3 = ResultBase.Fail(new NotFoundError("Player", 1));

        // Act
        var combined = ResultBase.Combine(result1, result2, result3);

        // Assert
        Assert.False(combined.IsSuccess);
        var composite = Assert.IsType<CompositeError>(combined.Error);
        Assert.Equal(2, composite.InnerErrors.Count);
    }

    #endregion
}