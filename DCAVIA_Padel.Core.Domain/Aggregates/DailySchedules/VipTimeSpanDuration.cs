using DCAVIA_Padel.Core.Domain.Common.Bases;
using DCAVIA_Padel.Core.Tools.OperationResult;
using DCAVIA_Padel.Core.Tools.OperationResult.Errors;

namespace DCAVIA_Padel.Core.Domain.Aggregates.DailySchedules;

public class VipTimeSpanDuration : ValueObject<VipTimeSpanDuration>
{
    internal int Value { get; }
    
    private VipTimeSpanDuration(int value) => Value = value;
    
    public static Result<VipTimeSpanDuration> Create(int value)
        => Create(() => new VipTimeSpanDuration(value));

    protected override Result Validate()
    {
        List<ResultError> errors = [];
        
        if (int.IsNegative(Value))
            errors.Add(new ValidationError("VIP_TIME_SPAN_DURATION", "VIP time span duration cannot be negative."));
        
        if (Value % 30 != 0)
            errors.Add(new ValidationError("VIP_TIME_SPAN_DURATION", "VIP time span duration must be in increments of 30 minutes."));
        
        if (Value is > 60 * 24 or 0)
            errors.Add(new ValidationError("VIP_TIME_SPAN_DURATION", "VIP time span duration must be between 30 minutes and 24 hours."));
        
        if (errors.Count == 0) return ResultBase.Ok();
        
        return errors.Count == 1
            ? ResultBase.Fail(errors[0])
            : ResultBase.Fail(new CompositeError(errors));
    }
}