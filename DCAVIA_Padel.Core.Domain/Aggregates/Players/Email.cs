using DCAVIA_Padel.Core.Domain.Common.Bases;
using DCAVIA_Padel.Core.Domain.Common.Values;
using DCAVIA_Padel.Core.Tools.OperationResult;
using DCAVIA_Padel.Core.Tools.OperationResult.Errors;

namespace DCAVIA_Padel.Core.Domain.Aggregates.Players;

public class Email : ValueObject
{
    internal string Value { get; }

    private Email(string value) => Value = value;

    public static Result<Email> Create(string value)
    {
        var email = new Email(value);
        var validation = email.Validate();

        return validation.IsFailure ? ResultBase.Fail<Email>(validation.Error!) : ResultBase.Ok(email);
    }

    public override Result Validate()
    {
        if (string.IsNullOrWhiteSpace(Value))
            return ResultBase.Fail(new ValidationError("MAIL", "Email cannot be empty."));

        List<ResultError> errors = [];

        var parts = Value.Split('@');

        if (parts.Length != 2)
        {
            errors.Add(new ValidationError("MAIL", "Email must contain exactly one '@'."));
        }
        else
        {
            var viaIdPart = parts[0];
            var domainPart = parts[1];

            var viaIdValidation = VIAID.Create(viaIdPart);
            if (viaIdValidation.IsFailure)
                errors.Add(viaIdValidation.Error!);

            if (domainPart != "via.dk")
                errors.Add(new ValidationError("MAIL", "Email domain must be 'via.dk'."));
        }

        if (errors.Count == 0) return ResultBase.Ok();

        return errors.Count == 1
            ? ResultBase.Fail(errors[0])
            : ResultBase.Fail(new CompositeError(errors));
    }
}