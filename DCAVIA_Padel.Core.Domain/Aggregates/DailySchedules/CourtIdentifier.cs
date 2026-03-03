using DCAVIA_Padel.Core.Domain.Common.Bases;
using DCAVIA_Padel.Core.Tools.OperationResult;
using DCAVIA_Padel.Core.Tools.OperationResult.Errors;

namespace DCAVIA_Padel.Core.Domain.Aggregates.DailySchedules;

public class CourtIdentifier : ValueObject<CourtIdentifier>
{
    internal string Value { get; }
    
    private const char SinglesPrefix = 'S';
    private const char DoublesPrefix = 'D';
    
    private CourtIdentifier(string value) => Value = value;
    
    public static Result<CourtIdentifier> Create(string value) 
        => Create(() => new CourtIdentifier(value));
    
    protected override Result Validate()
    {
        if (string.IsNullOrWhiteSpace(Value))
            return ResultBase.Fail(new ValidationError("COURT_IDENTIFIER", "Court identifier cannot be empty."));
        
        List<ValidationError> errors = [];
        
        if (!Value.StartsWith(SinglesPrefix) && !Value.StartsWith(DoublesPrefix))
            errors.Add(new ValidationError("COURT_IDENTIFIER", "Court identifier must start with 'S' for singles or 'D' for doubles."));
        switch (Value.Length)
        {
            case < 2:
            case > 3:
                errors.Add(new ValidationError("COURT_IDENTIFIER", "Court identifier must be 2 or 3 characters long."));
                break;
            case 2 when !char.IsDigit(Value[1]) || Value[1] == '0':
                errors.Add(new ValidationError("COURT_IDENTIFIER", "Court identifier with 2 characters must have a digit as the second character."));
                break;
            case 3 when (!char.IsDigit(Value[1]) || !char.IsDigit(Value[2])):
                errors.Add(new ValidationError("COURT_IDENTIFIER", "Court identifier with 3 characters must have digits as the second and third characters."));
                break;
        }


        if (errors.Count == 0) return ResultBase.Ok();
        
        return errors.Count == 1
            ? ResultBase.Fail(errors[0])
            : ResultBase.Fail(new CompositeError(errors));
    }
}