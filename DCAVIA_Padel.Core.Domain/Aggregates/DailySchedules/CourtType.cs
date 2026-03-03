using DCAVIA_Padel.Core.Domain.Common.Bases;
using DCAVIA_Padel.Core.Tools.OperationResult;
using DCAVIA_Padel.Core.Tools.OperationResult.Errors;

namespace DCAVIA_Padel.Core.Domain.Aggregates.DailySchedules;

public class CourtType : ValueObject<CourtType>
{
    internal CourtTypeEnum Value { get; }
    
    private CourtType(CourtTypeEnum value) => Value = value;

    public static Result<CourtType> Create(CourtTypeEnum value)
        => Create(() => new CourtType(value));

    protected override Result Validate()
    {
        return Enum.IsDefined(typeof(CourtTypeEnum), Value)
            ? ResultBase.Ok()
            : ResultBase.Fail(new ValidationError("COURT_TYPE", "Invalid court type."));
    }
}

public enum CourtTypeEnum
{
    Single, Double
}