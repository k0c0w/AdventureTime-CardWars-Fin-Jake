namespace GameObjects.Creatures;

public class CornWorm : Creature
{
    public CornWorm(int line, Player owner) : base(line, owner, LandType.CornFields, 1, 4, 5)
    {
        owner.Lands[line].TurnOver();
    }

    public override void ExecuteSkill() { }
}