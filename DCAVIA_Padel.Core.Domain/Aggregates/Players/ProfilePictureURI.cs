using DCAVIA_Padel.Core.Domain.Common.Bases;
using DCAVIA_Padel.Core.Tools.OperationResult;
using DCAVIA_Padel.Core.Tools.OperationResult.Errors;

namespace DCAVIA_Padel.Core.Domain.Aggregates.Players;

public class ProfilePictureURI : ValueObject
{
    internal string Value { get; private set; }
    
    private ProfilePictureURI(string value) => Value = value;
    
    public static Result<ProfilePictureURI> Create(string value)
    {
        var profilePictureUri = new ProfilePictureURI(value);
        var validation = profilePictureUri.Validate();

        return validation.IsFailure ? ResultBase.Fail<ProfilePictureURI>(validation.Error!) : ResultBase.Ok(profilePictureUri);
    }
    
    public override Result Validate()
    {
        if (string.IsNullOrWhiteSpace(Value))
            return ResultBase.Fail(new ValidationError("PROFILE_PICTURE_URI", "Profile picture URI cannot be empty."));
        
        return !Uri.IsWellFormedUriString(Value, UriKind.Absolute) 
            ? ResultBase.Fail(new ValidationError("PROFILE_PICTURE_URI", "Profile picture URI must be a valid absolute URI.")) 
            : ResultBase.Ok();
    }
}