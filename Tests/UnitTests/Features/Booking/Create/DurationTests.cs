using DCAVIA_Padel.Core.Domain.Aggregates.Bookings;

namespace UnitTests.Features.Booking.Create;

public class DurationTests
{
    [Theory]
    [InlineData(60)]
    [InlineData(90)]
    [InlineData(120)]
    [InlineData(150)]
    [InlineData(180)]
    [InlineData(210)]
    public void Create_ValidDuration_ReturnsSuccess(int validDuration)
    {
        // Act
        var result = Duration.Create(validDuration);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(validDuration, result.Value!.Value);
    }

    [Theory]
    [InlineData(30)]
    [InlineData(45)]
    [InlineData(142)]
    [InlineData(240)]
    [InlineData(0)]
    [InlineData(-60)]
    [InlineData(null)]
    public void Create_InvalidDuration_ReturnsFailure(int invalidDuration)
    {
        // Act
        var result = Duration.Create(invalidDuration);

        // Assert
        Assert.True(result.IsFailure);
    }
}