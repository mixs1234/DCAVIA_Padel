using DCAVIA_Padel.Core.Domain.Common.Bases;
using DCAVIA_Padel.Core.Tools.OperationResult;
using DCAVIA_Padel.Core.Tools.OperationResult.Errors;

namespace DCAVIA_Padel.Core.Domain.Aggregates.Players;

public class VIPStatusID : ValueObject
{
    internal Guid Value { get; private set; }
    
    private VIPStatusID(Guid guid) => Value =  guid;

    public static Result<VIPStatusID> Create(Guid guid)
    {
        var id = new VIPStatusID(guid);
        var validation = id.Validate();
        
        return validation.IsFailure ? ResultBase.Fail<VIPStatusID>(validation.Error!) : ResultBase.Ok(id);
    }
    
    public override Result Validate()
    {
        return ResultBase.Ok();
    }
    
    
}