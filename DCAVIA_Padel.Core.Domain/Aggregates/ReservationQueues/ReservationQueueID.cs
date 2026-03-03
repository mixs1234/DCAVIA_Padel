using DCAVIA_Padel.Core.Domain.Common.Bases;
using DCAVIA_Padel.Core.Tools.OperationResult;
using DCAVIA_Padel.Core.Tools.OperationResult.Errors;

namespace DCAVIA_Padel.Core.Domain.Aggregates.ReservationQueues;

public class ReservationQueueID : ValueObject<ReservationQueueID>
{
    private Guid Value { get; }
    
    private ReservationQueueID(Guid value) => Value = value;
    
    public static Result<ReservationQueueID> Create(Guid value) 
        => Create(() => new ReservationQueueID(value));

    protected override Result Validate()
    {
        return Value == Guid.Empty 
            ? ResultBase.Fail(new ValidationError("RESERVATION_QUEUE_ID", "Reservation Queue ID cannot be empty.")) 
            : ResultBase.Ok();
    }
}