namespace GameObjects.Creatures;

public class SpiritSolder : Creature
{
    public SpiritSolder(int line, Player owner) : base(line, owner, LandType.BluePlains, 1, 9, 1)
    { }

    public override void ExecuteSkill()
    {
        var bonus = new Bonus { AttackBonus = 1 };
        var creatures = Owner.Creatures;
        if(Line != 0)
            creatures[Line - 1]?.RegisterBonus(bonus);
        if(Line != 3)
            creatures[Line + 1]?.RegisterBonus(bonus);
    }
}