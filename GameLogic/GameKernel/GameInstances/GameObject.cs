using Shared.PossibleCards;

namespace GameObjects;

public abstract class GameObject
{
    public LandType LandType { get; }

    public AllCards Card { get; }
    
    public Player Owner { get; }

    public int SummonCost { get; }

    public GameObject(Player owner, LandType land, AllCards card, int cost)
    {
        if (owner == null)
            throw new ArgumentNullException($"The given {nameof(owner)} was null.");
        if (cost is < 0 or > 2)
            throw new ArgumentException($"The given {nameof(cost)} is incorrect: 0,1,2 only are allowed.");

        if (owner.Lands.Count(x => !x.IsTurnedOver && x.LandType == land) < cost)
            throw new InvalidOperationException("Not enough lands to summon creature.");
        
        SummonCost = cost;
        Owner = owner;
        LandType = land;
        Card = card;
    }

    //todo: переделать на исполнение action (так как есть карты, которые меняют действие)
    public abstract void ExecuteSkill();
}