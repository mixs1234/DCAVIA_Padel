using DCAVIA_Padel.Core.Domain.Common.Bases;
using DCAVIA_Padel.Core.Tools.OperationResult;
using DCAVIA_Padel.Core.Tools.OperationResult.Errors;

namespace DCAVIA_Padel.Core.Domain.Aggregates.ReservationQueues;

public class ReservationQueueEntryID : ValueObject<ReservationQueueEntryID>
{
    private Guid Value { get; }
    
    private ReservationQueueEntryID(Guid value) => Value = value;

    public static Result<ReservationQueueEntryID> Create(Guid value) 
        => Create(() => new ReservationQueueEntryID(value));
    
    protected override Result Validate()
    {
        return Value == Guid.Empty 
            ? ResultBase.Fail(new ValidationError("RESERVATION_QUEUE_ENTRY_ID", "Reservation Queue Entry ID cannot be empty.")) 
            : ResultBase.Ok();
    }
}