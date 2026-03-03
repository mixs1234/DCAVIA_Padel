using DCAVIA_Padel.Core.Domain.Common.Bases;
using DCAVIA_Padel.Core.Tools.OperationResult;
using DCAVIA_Padel.Core.Tools.OperationResult.Errors;

namespace DCAVIA_Padel.Core.Domain.Aggregates.Bookings;

public class BookingID : ValueObject<BookingID>
{
    private Guid Value { get; }
    
    private BookingID(Guid guid) => Value =  guid;
    
    public static Result<BookingID> Create(Guid guid) 
        => Create(() => new BookingID(guid));

    protected override Result Validate()
    {
        return Value == Guid.Empty 
            ? ResultBase.Fail(new ValidationError("BOOKING_ID", "Booking ID cannot be empty.")) 
            : ResultBase.Ok();
    }
}