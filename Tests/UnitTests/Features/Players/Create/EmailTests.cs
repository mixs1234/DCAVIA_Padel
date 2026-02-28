using DCAVIA_Padel.Core.Domain.Aggregates.Players;

namespace UnitTests.Features.Players.Create;

public class EmailTests
{
    [Fact]
    public void Create_ValidStudentEmail_ReturnsSuccess()
    {
        // Arrange
        var emailStr = "123456@via.dk";

        // Act
        var result = Email.Create(emailStr);
        
        // Assert
        Assert.True(result.IsSuccess);
        var email = result.Value;
        Assert.NotNull(email);
        Assert.Equal(emailStr, email.Value);
    }
    
    [Fact]
    public void Create_ValidEmployee1Email_ReturnsSuccess()
    {
        // Arrange
        var emailStr = "abc@via.dk";

        // Act
        var result = Email.Create(emailStr);
        
        // Assert
        Assert.True(result.IsSuccess);
        var email = result.Value;
        Assert.NotNull(email);
        Assert.Equal(emailStr, email.Value);
    }
    
    [Fact]
    public void Create_ValidEmployee2Email_ReturnsSuccess()
    {
        // Arrange
        var emailStr = "abcd@via.dk";

        // Act
        var result = Email.Create(emailStr);
        
        // Assert
        Assert.True(result.IsSuccess);
        var email = result.Value;
        Assert.NotNull(email);
        Assert.Equal(emailStr, email.Value);
    }
    
    [Fact]
    public void Create_InValidEmailDomain_ReturnsFails()
    {
        // Arrange
        var emailStr = "abcd@gmail.com";

        // Act
        var result = Email.Create(emailStr);
        
        // Assert
        Assert.True(result.IsFailure);
        var email = result.Value;
        Assert.Null(email);
    }
    
    [Fact]
    public void Create_InValidEmailName_ReturnsFails()
    {
        // Arrange
        var emailStr = "1234567@via.dk";

        // Act
        var result = Email.Create(emailStr);
        
        // Assert
        Assert.True(result.IsFailure);
        var email = result.Value;
        Assert.Null(email);
    }

    [Fact]
    public void Create_EmptyEmail_ReturnsFails()
    {
        // Arrange
        var emailStr = "";

        // Act
        var result = Email.Create(emailStr);

        // Assert
        Assert.True(result.IsFailure);
        var email = result.Value;
        Assert.Null(email);
    }

    [Fact]
    public void Create_NullEmail_ReturnsFails()
    {
        // Arrange
        string? emailStr = null;

        // Act
        var result = Email.Create(emailStr!);

        // Assert
        Assert.True(result.IsFailure);
        var email = result.Value;
        Assert.Null(email);
    }
}