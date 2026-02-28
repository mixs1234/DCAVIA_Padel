using DCAVIA_Padel.Core.Domain.Aggregates.Players;
using DCAVIA_Padel.Core.Domain.Common.Bases;

namespace UnitTests.Features.Players.Create;

public class ProfilePictureUriTests
{
    [Theory]
    [InlineData("http://example.com/image.jpg")]
    [InlineData("https://example.com/image.jpg")]
    public void Create_ValidProfilePictureUri_ReturnsSuccess(string uriStr)
    {
        // Act
        var result = ProfilePictureURI.Create(uriStr);
        
        // Assert
        Assert.True(result.IsSuccess);
        var profilePictureUri = result.Value;
        Assert.NotNull(profilePictureUri);
        Assert.Equal(uriStr, profilePictureUri.Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("invalid_uri")]
    public void Create_InvalidProfilePictureUri_ReturnsFails(string uriStr)
    {
        // Act
        var result = ProfilePictureURI.Create(uriStr);
        
        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
    }

}