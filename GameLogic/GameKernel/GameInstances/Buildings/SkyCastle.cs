using Shared.PossibleCards;

namespace GameObjects.Buildings;

public class SkyCastle : Building
{
    public SkyCastle(int line, Player owner) : base(line, owner, LandType.any, AllCards.celestial_castle, 1)
    {
    }

    public override void ExecuteSkill()
    {
        Owner.Creatures[Line]?.RegisterBonus(new Bonus { DefenceBonus = 3 });
    }
}