namespace GameObjects.Creatures;

public class CornKnight : Creature
{
    public CornKnight(int line, Player owner, LandType land) : base(line, owner, land, 1, 0, 0)
    { }

    public override void ExecuteSkill()
    {
        var controlledCornFields = Owner.ControlledLands(LandType);
        RegisterBonus(new Bonus(2 * controlledCornFields, controlledCornFields));
    }
}