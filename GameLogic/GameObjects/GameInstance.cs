namespace GameObjects;

public abstract class GameInstance
{
    //todo: как индетефицировать сущность в пакете?? 
    //public Guid SessionInstanceId { get; init; } = Guid.NewGuid() ???
    public LandType LandType { get; }
    
    public IPlayer Owner { get; }

    public int SummonCost { get; }

    public GameInstance(IPlayer owner, LandType land, int cost)
    {
        if (owner == null)
            throw new ArgumentNullException($"The given {nameof(owner)} was null.");
        if (cost < 0 || cost > 2)
            throw new ArgumentException($"The given {nameof(cost)} is incorrect: 0,1,2 only are allowed.");

        SummonCost = cost;
        Owner = owner;
        LandType = land;
    }
}