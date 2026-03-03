using DCAVIA_Padel.Core.Domain.Common.Bases;
using DCAVIA_Padel.Core.Tools.OperationResult;

namespace DCAVIA_Padel.Core.Domain.Aggregates.Bookings;

public class Duration : ValueObject
{
    internal int Value { get; private set; }
    
    private Duration(int value) => Value = value;
    
    public static Result<Duration> Create(int value)
    {
        var duration = new Duration(value);
        var validation = duration.Validate();

        return validation.IsFailure ? ResultBase.Fail<Duration>(validation.Error!) : ResultBase.Ok(duration);
    }
    
    public override Result Validate()
    {
        if ()
        
    }
}