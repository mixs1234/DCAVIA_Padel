using DCAVIA_Padel.Core.Domain.Common.Values;
using DCAVIA_Padel.Core.Tools.OperationResult.Errors;

namespace UnitTests.Common.Create;

public class VIAIDTest
{
    [Theory]
    [InlineData("123456")]
    [InlineData("abcd")]
    [InlineData("abc")]
    public void Create_ValidVIAID_ReturnsSuccess(string validViaId)
    {
        // Act
        var result = VIAID.Create(validViaId);

        // Assert
        Assert.True(result.IsSuccess);
        var viaId = result.Value;
        Assert.NotNull(viaId);
        Assert.Equal(validViaId, viaId.Value);
    }

    [Theory]
    [InlineData("12345")]
    [InlineData("1234567")] 
    [InlineData("abcdef")] 
    [InlineData("123abc")]
    public void Create_InvalidVIAID_ReturnsFailure(string invalidViaId)
    {
        // Act
        var result = VIAID.Create(invalidViaId);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
    }
    
    [Fact]
    public void Create_NullVIAID_ReturnsFailure()
    {
        // Arrange
        string? invalidViaId = null;
        
        // Act
        var result = VIAID.Create(invalidViaId!);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
    }
    
    [Fact]
    public void Create_VIAIDWithCompositeError_ReturnsFailureWithAllErrors()
    {
        // Arrange
        var invalidViaId = "ABCDE";
        
        // Act
        var result = VIAID.Create(invalidViaId);
        
        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
        var compositeError = result.Error as CompositeError;
        Assert.NotNull(compositeError);
        Assert.Equal(2, compositeError.InnerErrors.Count);
    }






}