using DCAVIA_Padel.Core.Domain.Common.Bases;
using DCAVIA_Padel.Core.Tools.OperationResult;
using DCAVIA_Padel.Core.Tools.OperationResult.Errors;

namespace DCAVIA_Padel.Core.Domain.Aggregates.Bookings;

public class Duration : ValueObject<Duration>
{
    internal int Value { get; }
    
    private Duration(int value) => Value = value;
    
    public static Result<Duration> Create(int value)
        => Create(() => new Duration(value));

    protected override Result Validate()
    {
        List<ResultError> errors = [];
        
        if (int.IsNegative(Value))
            errors.Add(new ValidationError("DURATION", "Duration cannot be negative."));
        
        if (Value % 30 != 0)
            errors.Add(new ValidationError("DURATION", "Duration must be in increments of 30 minutes."));
        
        if (Value < 60 || Value > 210)
            errors.Add(new ValidationError("DURATION", "Duration must be between 60 and 210 minutes."));
        
        if (errors.Count == 0) return ResultBase.Ok();
        
        return errors.Count == 1
            ? ResultBase.Fail(errors[0])
            : ResultBase.Fail(new CompositeError(errors));
        
    }
}