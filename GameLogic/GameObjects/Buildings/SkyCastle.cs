namespace GameObjects.Buildings;

public class SkyCastle : Building
{
    public SkyCastle(int line, Player owner) : base(line, owner, LandType.Any, 1)
    {
    }

    public override void ExecuteSkill()
    {
        Owner.Creatures[Line]?.RegisterBonus(new Bonus { DefenceBonus = 3 });
    }
}