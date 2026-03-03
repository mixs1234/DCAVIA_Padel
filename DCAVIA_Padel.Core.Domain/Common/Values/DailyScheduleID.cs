using DCAVIA_Padel.Core.Domain.Common.Bases;
using DCAVIA_Padel.Core.Tools.OperationResult;
using DCAVIA_Padel.Core.Tools.OperationResult.Errors;

namespace DCAVIA_Padel.Core.Domain.Common.Values;

public class DailyScheduleID: ValueObject<DailyScheduleID>
{
    private Guid Value { get; }
    
    private DailyScheduleID(Guid value) => Value = value;
    
    public static Result<DailyScheduleID> Create(Guid value) 
        => Create(() => new DailyScheduleID(value));
    
    protected override Result Validate()
    {
        return Value == Guid.Empty 
            ? ResultBase.Fail(new ValidationError("DAILY_SCHEDULE_ID", "Daily Schedule ID cannot be empty.")) 
            : ResultBase.Ok();
    }
}