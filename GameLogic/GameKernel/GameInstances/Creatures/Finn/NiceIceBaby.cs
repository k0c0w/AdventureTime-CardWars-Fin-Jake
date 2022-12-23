using Shared.PossibleCards;

namespace GameObjects.Creatures;

public class NiceIceBaby : Creature
{
    public NiceIceBaby(int line, Player owner) : base(line, owner, LandType.any, AllCards.nice_ice_baby, 0, 2, 1)
    {
    }

    public override void ExecuteSkill()
    {
        if(Owner.Opponent.Creatures[Line] == null)
            RegisterBonus(new Bonus {AttackBonus = 3});
    }
}