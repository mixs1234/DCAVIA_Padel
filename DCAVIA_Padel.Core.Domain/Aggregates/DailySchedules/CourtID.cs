using DCAVIA_Padel.Core.Domain.Common.Bases;
using DCAVIA_Padel.Core.Tools.OperationResult;
using DCAVIA_Padel.Core.Tools.OperationResult.Errors;

namespace DCAVIA_Padel.Core.Domain.Aggregates.DailySchedules;

public class CourtID : ValueObject<CourtID>
{
    private Guid Value { get; }
    
    private CourtID(Guid value) => Value = value;
    
    public static Result<CourtID> Create(Guid value) 
        => Create(() => new CourtID(value));
    
    protected override Result Validate()
    {
        return Value == Guid.Empty 
            ? ResultBase.Fail(new ValidationError("COURT_ID", "Court ID cannot be empty.")) 
            : ResultBase.Ok();
    }
}