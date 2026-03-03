using DCAVIA_Padel.Core.Domain.Common.Bases;
using DCAVIA_Padel.Core.Tools.OperationResult;
using DCAVIA_Padel.Core.Tools.OperationResult.Errors;

namespace DCAVIA_Padel.Core.Domain.Aggregates.Players;

public class ProfilePictureUri : ValueObject<ProfilePictureUri>
{
    internal string Value { get; }
    
    private ProfilePictureUri(string value) => Value = value;
    
    public static Result<ProfilePictureUri> Create(string value) 
        => Create(() => new ProfilePictureUri(value));

    protected override Result Validate()
    {
        if (string.IsNullOrWhiteSpace(Value))
            return ResultBase.Fail(new ValidationError("PROFILE_PICTURE_URI", "Profile picture URI cannot be empty."));
        
        return !Uri.IsWellFormedUriString(Value, UriKind.Absolute) 
            ? ResultBase.Fail(new ValidationError("PROFILE_PICTURE_URI", "Profile picture URI must be a valid absolute URI.")) 
            : ResultBase.Ok();
    }
}