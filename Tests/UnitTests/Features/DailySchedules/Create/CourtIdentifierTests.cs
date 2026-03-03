using DCAVIA_Padel.Core.Domain.Aggregates.DailySchedules;

namespace UnitTests.Features.DailySchedules.Create;

public class CourtIdentifierTests
{
    [Theory]
    [InlineData("S1")]
    [InlineData("S2")]
    [InlineData("S10")]
    [InlineData("D1")]
    [InlineData("D2")]
    [InlineData("D10")]
    public void Create_ValidCourtIdentifier_ReturnsSuccess(string validCourtIdentifier)
    {
        // Act
        var result = CourtIdentifier.Create(validCourtIdentifier);
        
        // Assert
        Assert.True(result.IsSuccess);
        var courtIdentifier = result.Value;
        Assert.NotNull(courtIdentifier);
        Assert.Equal(validCourtIdentifier, courtIdentifier.Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("X1")]
    [InlineData("S0")]
    [InlineData("D0")]
    [InlineData("S1d")]
    [InlineData("S1D")]
    [InlineData("S-1")]
    [InlineData("S1.5")]
    [InlineData("S")]
    [InlineData("D")]
    [InlineData("1S")]
    [InlineData(null)]
    public void Create_InvalidCourtIdentifier_ReturnsFails(string invalidCourtIdentifier)
    {
        // Act
        var result = CourtIdentifier.Create(invalidCourtIdentifier);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
    }
}