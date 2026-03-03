using DCAVIA_Padel.Core.Tools.OperationResult;

namespace DCAVIA_Padel.Core.Domain.Common.Bases;

public abstract class ValueObject<T> where T : ValueObject<T>
{
    protected abstract Result Validate();
    
    protected static Result<T> Create(Func<T> factory)
    {
        var instance = factory();
        var validation = instance.Validate();
        
        return validation.IsFailure 
            ? ResultBase.Fail<T>(validation.Error!) 
            : ResultBase.Ok(instance);
    }
}