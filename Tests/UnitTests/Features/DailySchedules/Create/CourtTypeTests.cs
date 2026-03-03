using DCAVIA_Padel.Core.Domain.Aggregates.DailySchedules;

namespace UnitTests.Features.DailySchedules.Create;

public class CourtTypeTests
{
    [Theory]
    [InlineData(CourtTypeEnum.Single)]
    [InlineData(CourtTypeEnum.Double)]
    public void Create_ValidCourtType_ReturnsSuccess(CourtTypeEnum validCourtType)
    {
        // Act
        var result = CourtType.Create(validCourtType);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(validCourtType, result.Value!.Value);
    }
    
    [Theory]
    [InlineData((CourtTypeEnum)999)]
    [InlineData((CourtTypeEnum)(-1))]
    public void Create_InvalidCourtType_ReturnsFailure(CourtTypeEnum invalidCourtType)
    {
        // Act
        var result = CourtType.Create(invalidCourtType);

        // Assert
        Assert.True(result.IsFailure);
    }
}