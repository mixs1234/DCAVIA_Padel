namespace DCAVIA_Padel.Core.Domain.Aggregates.Players;

public class VIPStatusID
{
    internal Guid Value { get; private set; } = Guid.NewGuid();
}