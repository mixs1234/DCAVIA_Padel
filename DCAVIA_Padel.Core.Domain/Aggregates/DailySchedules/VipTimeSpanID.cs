using DCAVIA_Padel.Core.Domain.Common.Bases;
using DCAVIA_Padel.Core.Tools.OperationResult;
using DCAVIA_Padel.Core.Tools.OperationResult.Errors;

namespace DCAVIA_Padel.Core.Domain.Aggregates.DailySchedules;

public class VipTimeSpanID: ValueObject<VipTimeSpanID>
{
    private Guid Value { get; }
    
    private VipTimeSpanID(Guid value) => Value = value;
    
    public static Result<VipTimeSpanID> Create(Guid value) 
        => Create(() => new VipTimeSpanID(value));
    
    protected override Result Validate()
    {
        return Value == Guid.Empty 
            ? ResultBase.Fail(new ValidationError("VIP_TIME_SPAN_ID", "VIP Time Span ID cannot be empty.")) 
            : ResultBase.Ok();
    }
}