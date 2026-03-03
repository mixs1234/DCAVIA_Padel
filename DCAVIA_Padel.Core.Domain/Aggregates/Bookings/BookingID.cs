using DCAVIA_Padel.Core.Domain.Common.Bases;
using DCAVIA_Padel.Core.Tools.OperationResult;

namespace DCAVIA_Padel.Core.Domain.Aggregates;

public class BookingID : ValueObject
{
    internal Guid Value { get; private set; }
    
    private BookingID(Guid guid) => Value =  guid;
    
    public static Result<BookingID> Create(Guid guid)
    {
        var id = new BookingID(guid);
        var validation = id.Validate();
        
        return validation.IsFailure ? ResultBase.Fail<BookingID>(validation.Error!) : ResultBase.Ok(id);
    }
    
    public override Result Validate()
    {
        return ResultBase.Ok();
    }
}