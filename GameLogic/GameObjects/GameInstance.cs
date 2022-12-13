namespace GameObjects;

public abstract class GameInstance
{
    //todo: как индетефицировать сущность в пакете?? 
    //public Guid SessionInstanceId { get; init; } = Guid.NewGuid() ???
    public LandType LandType { get; }
    
    public Player Owner { get; }

    public int SummonCost { get; }

    public GameInstance(Player owner, LandType land, int cost)
    {
        if (owner == null)
            throw new ArgumentNullException($"The given {nameof(owner)} was null.");
        if (cost < 0 || cost > 2)
            throw new ArgumentException($"The given {nameof(cost)} is incorrect: 0,1,2 only are allowed.");

        if (owner.Lands.Count(x => !x.IsTurnedOver && x.LandType == land) < cost)
            throw new InvalidOperationException("Not enough lands to summon creature.");
        
        SummonCost = cost;
        Owner = owner;
        LandType = land;
    }

    //todo: переделать на исполнение action (так как есть карты, которые меняют действие)
    public abstract void ExecuteSkill();
}