using DCAVIA_Padel.Core.Tools.OperationResult;
using DCAVIA_Padel.Core.Tools.OperationResult.Errors;

namespace UnitTests.OperationResult;

public class ResultErrorTests
{
    #region Base ResultError Properties

    [Fact]
    public void ConflictError_StoresCodeAndMessage()
    {
        // Arrange & Act
        var error = new ConflictError("Duplicate entry");

        // Assert
        Assert.Equal("CONFLICT", error.ErrorCode);
        Assert.Equal("Duplicate entry", error.Message);
    }

    [Fact]
    public void NotFoundError_StoresEntityAndId()
    {
        // Arrange & Act
        var error = new NotFoundError("Player", 42);

        // Assert
        Assert.Equal("NOT_FOUND", error.ErrorCode);
        Assert.Contains("Player", error.Message);
        Assert.Contains("42", error.Message);
        Assert.Equal("Player", error.EntityName);
        Assert.Equal(42, error.Id);
    }

    [Fact]
    public void UnauthorizedError_StoresDefaultMessage()
    {
        // Arrange & Act
        var error = new UnauthorizedError();

        // Assert
        Assert.Equal("UNAUTHORIZED", error.ErrorCode);
        Assert.NotEmpty(error.Message);
    }

    [Fact]
    public void UnauthorizedError_StoresCustomMessage()
    {
        // Arrange & Act
        var error = new UnauthorizedError("Custom auth message");

        // Assert
        Assert.Equal("UNAUTHORIZED", error.ErrorCode);
        Assert.Equal("Custom auth message", error.Message);
    }

    #endregion

    #region ValidationError

    [Fact]
    public void ValidationError_SingleDetail_StoresFieldAndMessage()
    {
        // Arrange & Act
        var error = new ValidationError("Email", "Email is required");

        // Assert
        Assert.Equal("VALIDATION_ERROR", error.ErrorCode);
        Assert.Single(error.Details);
        Assert.Equal("Email", error.Details[0].Field);
        Assert.Equal("Email is required", error.Details[0].Message);
    }

    [Fact]
    public void ValidationError_MultipleDetails_StoresAll()
    {
        // Arrange
        var details = new List<ValidationDetail>
        {
            new("Name", "Name is required"),
            new("Email", "Email is invalid"),
            new("Age", "Must be 18+")
        };

        // Act
        var error = new ValidationError(details);

        // Assert
        Assert.Equal("VALIDATION_ERROR", error.ErrorCode);
        Assert.Equal(3, error.Details.Count);
    }

    #endregion

    #region CompositeError

    [Fact]
    public void CompositeError_CombinesMultipleErrors()
    {
        // Arrange
        var errors = new ResultError[]
        {
            new ConflictError("Conflict happened"),
            new NotFoundError("Player", 1),
            new UnauthorizedError()
        };

        // Act
        var composite = new CompositeError(errors);

        // Assert
        Assert.Equal("COMPOSITE_ERROR", composite.ErrorCode);
        Assert.Equal(3, composite.InnerErrors.Count);
    }

    [Fact]
    public void CompositeError_PreservesInnerErrorTypes()
    {
        // Arrange
        var conflict = new ConflictError("Duplicate");
        var notFound = new NotFoundError("Match", 99);

        // Act
        var composite = new CompositeError(new ResultError[] { conflict, notFound });

        // Assert
        Assert.IsType<ConflictError>(composite.InnerErrors[0]);
        Assert.IsType<NotFoundError>(composite.InnerErrors[1]);
    }

    [Fact]
    public void CompositeError_EmptyList_HasNoInnerErrors()
    {
        // Act
        var composite = new CompositeError(Array.Empty<ResultError>());

        // Assert
        Assert.Empty(composite.InnerErrors);
    }

    #endregion

    #region ToString

    [Fact]
    public void ToString_FormatsErrorCorrectly()
    {
        // Arrange
        var error = new ConflictError("Already exists");

        // Act
        var result = error.ToString();

        // Assert
        Assert.Contains("CONFLICT", result);
        Assert.Contains("Already exists", result);
    }

    #endregion
}