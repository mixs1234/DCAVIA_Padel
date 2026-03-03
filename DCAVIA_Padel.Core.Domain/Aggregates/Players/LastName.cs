using DCAVIA_Padel.Core.Domain.Common.Bases;
using DCAVIA_Padel.Core.Tools.OperationResult;
using DCAVIA_Padel.Core.Tools.OperationResult.Errors;

namespace DCAVIA_Padel.Core.Domain.Aggregates.Players;

public class LastName : ValueObject<LastName>
{
    internal string Value { get; }
    
    private LastName(string value) => Value = value;
    
    public static Result<LastName> Create(string value) 
        => Create(() => new LastName(value));


    protected override Result Validate()
    {
        if (string.IsNullOrWhiteSpace(Value))
            return ResultBase.Fail(new ValidationError("LAST_NAME", "Last name cannot be empty."));
        
        List<ResultError> errors = [];
        if (Value.Length > 50)
            errors.Add(new ValidationError("LAST_NAME", "Last name cannot exceed 50 characters."));
        
        if (Value.Any(ch => !char.IsLetter(ch)))
            errors.Add(new ValidationError("LAST_NAME", "Last name can only contain letters."));
        
        if (errors.Count == 0) return ResultBase.Ok();
        
        return errors.Count == 1
            ? ResultBase.Fail(errors[0])
            : ResultBase.Fail(new CompositeError(errors));
    }
}