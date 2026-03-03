using DCAVIA_Padel.Core.Domain.Common.Bases;
using DCAVIA_Padel.Core.Tools.OperationResult;
using DCAVIA_Padel.Core.Tools.OperationResult.Errors;

namespace DCAVIA_Padel.Core.Domain.Aggregates.Players;

public class VipStatusID : ValueObject<VipStatusID>
{
    private Guid Value { get; }
    
    private VipStatusID(Guid guid) => Value =  guid;

    public static Result<VipStatusID> Create(Guid guid) 
        => Create(() => new VipStatusID(guid));

    protected override Result Validate()
    {
        return Value == Guid.Empty 
            ? ResultBase.Fail(new ValidationError("VIP_STATUS_ID", "VIP Status ID cannot be empty.")) 
            : ResultBase.Ok();
    }
    
    
}