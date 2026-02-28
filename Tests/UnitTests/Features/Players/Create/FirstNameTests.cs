using DCAVIA_Padel.Core.Domain.Aggregates.Players;

namespace UnitTests.Features.Players.Create;

public class FirstNameTests
{
    [Theory]
    [InlineData("John")]
    [InlineData("Alice")]
    public void Create_ValidFirstName_ReturnsSuccess(string firstName)
    {
        // Act
        var result = FirstName.Create(firstName);

        // Assert
        Assert.True(result.IsSuccess);
        var firstNameObj = result.Value;
        Assert.NotNull(firstNameObj);
        Assert.Equal(firstName, firstNameObj.Value);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData("ThisIsAVeryLongFirstNameThatExceedsTheMaximumAllowedLength")]
    [InlineData("John123")]
    [InlineData("Jane_Doe")]
    [InlineData("John Doe")]
    [InlineData("!@#$%^&*()")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Create_InValidFirstName_ReturnsFails(string firstName)
    {
        // Act
        var result = FirstName.Create(firstName);

        // Assert
        Assert.False(result.IsSuccess);
    }
}