using DCAVIA_Padel.Core.Domain.Aggregates.DailySchedules;

namespace UnitTests.Features.DailySchedules.Create;

public class VipTimeSpanDurationTests
{
    [Theory]
    [InlineData(30)]
    [InlineData(60)]
    [InlineData(90)]
    [InlineData(120)]
    public void Create_ValidVipTimeSpanDuration_ReturnsSuccess(int validDuration)
    {
        // Act
        var result = VipTimeSpanDuration.Create(validDuration);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(validDuration, result.Value!.Value);
    }

    [Theory]
    [InlineData(15)]
    [InlineData(45)]
    [InlineData(75)]
    [InlineData(135)]
    [InlineData(0)]
    [InlineData(-30)]
    [InlineData(null)]
    public void Create_InvalidVipTimeSpanDuration_ReturnsFailure(int invalidDuration)
    {
        // Act
        var result = VipTimeSpanDuration.Create(invalidDuration);

        // Assert
        Assert.True(result.IsFailure);
    }
}