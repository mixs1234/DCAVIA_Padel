using DCAVIA_Padel.Core.Tools.OperationResult;

namespace DCAVIA_Padel.Core.Domain.Common.Bases;

public abstract class ValueObject
{
    public abstract Result Validate();
}