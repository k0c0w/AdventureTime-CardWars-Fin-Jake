namespace GameObjects.Buildings;

public class BloodyFortress : Building
{
    public BloodyFortress(int line, Player owner) : base(line, owner, LandType.Any, 1)
    {
    }

    public override void ExecuteSkill()
    {
        Owner.Creatures[Line]?.RegisterBonus(new Bonus { AttackBonus = 1 });
    }
}