namespace GameObjects.Creatures;

public class KingOfFields : Creature
{

    public KingOfFields(int line, Player owner) : base(line, owner, LandType.CornFields, 1, 7, 0)
    { }

    public override void ExecuteSkill() 
        => RegisterBonus(new Bonus { AttackBonus = Owner.Creatures.Count(x => x?.LandType == LandType.CornFields)});
}