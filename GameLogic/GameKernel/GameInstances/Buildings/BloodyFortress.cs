using Shared.PossibleCards;

namespace GameObjects.Buildings;

public class BloodyFortress : Building
{
    public BloodyFortress(int line, Player owner) : base(line, owner, LandType.any, AllCards.blood_fortress, 1)
    {
    }

    public override void ExecuteSkill()
    {
        Owner.Creatures[Line]?.RegisterBonus(new Bonus { AttackBonus = 1 });
    }
}