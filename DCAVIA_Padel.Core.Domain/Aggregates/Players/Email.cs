using DCAVIA_Padel.Core.Domain.Common.Bases;
using DCAVIA_Padel.Core.Domain.Common.Values;
using DCAVIA_Padel.Core.Tools.OperationResult;

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
            return ResultBase.Fail(new ResultError("422", "Email cannot be empty."));

        List<ResultError> errors = [];

        var parts = Value.Split('@');

        if (parts.Length != 2)
        {
            errors.Add(new ResultError("422", "Email must contain exactly one '@'."));
        }
        else
        {
            var viaIdPart = parts[0];
            var domainPart = parts[1];

            var viaIdValidation = VIAID.Create(viaIdPart);
            if (viaIdValidation.IsFailure)
                errors.Add(new ResultError("422", $"Email prefix is not a valid VIA ID: {viaIdValidation.Error?.ErrorMessages}"));

            if (domainPart != "via.dk")
                errors.Add(new ResultError("422", "Email domain must be 'via.dk'."));
        }

        if (errors.Count == 0) return ResultBase.Ok();

        var returnErrors = ResultError.Merge("422", errors);
        return ResultBase.Fail(returnErrors);
    }
}