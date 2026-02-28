using DCAVIA_Padel.Core.Domain.Common.Bases;
using DCAVIA_Padel.Core.Tools.OperationResult;
using DCAVIA_Padel.Core.Tools.OperationResult.Errors;

namespace DCAVIA_Padel.Core.Domain.Common.Values;

public class VIAID : ValueObject
{
    internal string Value { get; }
    
    private VIAID(string value) => Value = value;

    public static Result<VIAID> Create(string value)
    {
        var viaId = new VIAID(value);
        var validation = viaId.Validate();

        return validation.IsFailure 
            ? ResultBase.Fail<VIAID>(validation.Error!) 
            : ResultBase.Ok(viaId);
    }
    
    public override Result Validate()
    {
        if (string.IsNullOrWhiteSpace(Value))
            return ResultBase.Fail(new ValidationError("VIAID", "VIA ID cannot be empty."));

        List<ResultError> errors = [];

        bool isAllLetters = Value.All(char.IsLetter);
        bool isAllDigits = Value.All(char.IsDigit);

        switch (isAllLetters)
        {
            case false when !isAllDigits:
                errors.Add(new ValidationError("VIAID", "VIA ID must be either only letters or only numbers."));
                break;
            case true:
            {
                if (Value.Length < 3 || Value.Length > 4)
                    errors.Add(new ValidationError("VIAID", "VIA ID with letters must be between 3 and 4 characters long."));

                if (Value.Any(char.IsUpper))
                    errors.Add(new ValidationError("VIAID", "VIA ID with letters must be lowercase only."));
                break;
            }
            default:
            {
                if (isAllDigits && Value.Length != 6)
                    errors.Add(new ValidationError("VIAID", "VIA ID with numbers must be exactly 6 digits long."));
                break;
            }
        }

        if (errors.Count == 0) return ResultBase.Ok();

        return errors.Count == 1
            ? ResultBase.Fail(errors[0])
            : ResultBase.Fail(new CompositeError(errors));
    }
}