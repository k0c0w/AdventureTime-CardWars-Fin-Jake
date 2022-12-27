using Shared.PossibleCards;

namespace GameObjects.Creatures;

public class ShyBard : Creature, IFloopable
{
    public ShyBard(int line, Player owner) : base(line, owner, LandType.blue_plains, AllCards.shy_bard, 2, 5, 1)
    { }

    public override void ExecuteSkill()
    {
        if(!IsFlooped()) return;
        var count = Owner.Creatures.Count(x => x != null);
        //todo: игрок тянет карту здесь в колве count
    }

    public bool CanBeFlooped()
    {
        throw new NotImplementedException();
    }

    public void Floop()
    {
        throw new NotImplementedException();
    }

    public bool IsFlooped()
    {
        throw new NotImplementedException();
    }
}