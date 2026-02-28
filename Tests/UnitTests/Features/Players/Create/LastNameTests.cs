using DCAVIA_Padel.Core.Domain.Aggregates.Players;

namespace UnitTests.Features.Players.Create;

public class LastNameTests
{
    [Theory]
    [InlineData("Smith")]
    [InlineData("Johnson")]
    public void Create_ValidLastName_ReturnsSuccess(string lastName)
    {
        // Act
        var result = LastName.Create(lastName);

        // Assert
        Assert.True(result.IsSuccess);
        var lastNameObj = result.Value;
        Assert.NotNull(lastNameObj);
        Assert.Equal(lastName, lastNameObj.Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData("ThisIsAVeryLongLastNameThatExceedsTheMaximumAllowedLength")]
    [InlineData("Smith123")]
    [InlineData("Smith_Doe")]
    [InlineData("Smith Doe")]
    [InlineData("!@#$%^&*()")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Create_InValidLastName_ReturnsFails(string lastName)
    {
        // Act
        var result = LastName.Create(lastName);

        // Assert
        Assert.False(result.IsSuccess);
    }
}