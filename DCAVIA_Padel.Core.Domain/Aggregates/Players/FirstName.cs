using DCAVIA_Padel.Core.Domain.Common.Bases;
using DCAVIA_Padel.Core.Tools.OperationResult;
using DCAVIA_Padel.Core.Tools.OperationResult.Errors;

namespace DCAVIA_Padel.Core.Domain.Aggregates.Players;

public class FirstName : ValueObject
{
    internal string Value { get; private set; }
    
    private FirstName(string value) => Value = value;
    
    public static Result<FirstName> Create(string value)
    {
        var firstName = new FirstName(value);
        var validation = firstName.Validate();

        return validation.IsFailure ? ResultBase.Fail<FirstName>(validation.Error!) : ResultBase.Ok(firstName);
    }
    
    public override Result Validate()
    {
        if (string.IsNullOrWhiteSpace(Value))
            return ResultBase.Fail(new ValidationError("FIRST_NAME", "First name cannot be empty."));
        
        List<ResultError> errors = [];
        if (Value.Length > 50)
            errors.Add(new ValidationError("FIRST_NAME", "First name cannot exceed 50 characters."));
        
        if (Value.Any(ch => !char.IsLetter(ch)))
            errors.Add(new ValidationError("FIRST_NAME", "First name can only contain letters."));
        
        if (errors.Count == 0) return ResultBase.Ok();
        
        return errors.Count == 1
            ? ResultBase.Fail(errors[0])
            : ResultBase.Fail(new CompositeError(errors));
    }
}