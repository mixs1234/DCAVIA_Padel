using DCAVIA_Padel.Core.Tools.OperationResult;
using DCAVIA_Padel.Core.Tools.OperationResult.Errors;

namespace UnitTests.OperationResult;


/// <summary>
/// Tests for ResultError - verifies error codes, messages, and merging.
/// </summary>
public class ResultErrorTests
{
    #region Error Codes Requirement

    [Fact]
    public void Constructor_SingleMessage_StoresCodeAndMessage()
    {
        // Arrange
        var code = "TEST_ERROR";
        var message = "This is a test error";

        // Act
        var error = new ResultError(code, message);

        // Assert
        Assert.Equal(code, error.ErrorCode);
        Assert.Single(error.ErrorMessages);
        Assert.Contains(message, error.ErrorMessages);
    }

    [Fact]
    public void Constructor_MultipleMessages_StoresAllMessages()
    {
        // Arrange
        var code = "VALIDATION_ERROR";
        var messages = new List<string> { "Error 1", "Error 2", "Error 3" };

        // Act
        var error = new ResultError(code, messages);

        // Assert
        Assert.Equal(code, error.ErrorCode);
        Assert.Equal(3, error.ErrorMessages.Count);
        Assert.Equal(messages, error.ErrorMessages);
    }

    [Fact]
    public void Constructor_WithStackTrace_StoresStackTrace()
    {
        // Arrange
        var code = "EXCEPTION_ERROR";
        var message = "Something went wrong";
        var stackTrace = "at SomeClass.SomeMethod()";

        // Act
        var error = new ResultError(code, message, stackTrace);

        // Assert
        Assert.Equal(stackTrace, error.StackTrace);
    }

    #endregion

    #region Multiple Errors Requirement

    [Fact]
    public void Merge_MultipleErrors_CombinesAllMessages()
    {
        // Arrange
        var error1 = new ResultError("ERR1", new List<string> { "Message 1", "Message 2" });
        var error2 = new ResultError("ERR2", "Message 3");
        var error3 = new ResultError("ERR3", new List<string> { "Message 4", "Message 5" });

        var errors = new[] { error1, error2, error3 };

        // Act
        var merged = ResultError.Merge("MERGED_ERROR", errors);

        // Assert
        Assert.Equal("MERGED_ERROR", merged.ErrorCode);
        Assert.Equal(5, merged.ErrorMessages.Count);
        Assert.Contains("Message 1", merged.ErrorMessages);
        Assert.Contains("Message 2", merged.ErrorMessages);
        Assert.Contains("Message 3", merged.ErrorMessages);
        Assert.Contains("Message 4", merged.ErrorMessages);
        Assert.Contains("Message 5", merged.ErrorMessages);
    }

    [Fact]
    public void Merge_EmptyList_ReturnsEmptyMessages()
    {
        // Act
        var merged = ResultError.Merge("EMPTY", Array.Empty<ResultError>());

        // Assert
        Assert.Equal("EMPTY", merged.ErrorCode);
        Assert.Empty(merged.ErrorMessages);
    }

    [Fact]
    public void Merge_SingleError_PreservesMessages()
    {
        // Arrange
        var error = new ResultError("TEST", new List<string> { "Only message" });

        // Act
        var merged = ResultError.Merge("MERGED", new[] { error });

        // Assert
        Assert.Single(merged.ErrorMessages);
        Assert.Equal("Only message", merged.ErrorMessages.First());
    }

    #endregion

    #region ToString Test

    [Fact]
    public void ToString_FormatsErrorCorrectly()
    {
        // Arrange
        var error = new ResultError("TEST_CODE", new List<string> { "Msg1", "Msg2" });

        // Act
        var result = error.ToString();

        // Assert
        Assert.Contains("TEST_CODE", result);
        Assert.Contains("Msg1", result);
        Assert.Contains("Msg2", result);
    }

    #endregion
}