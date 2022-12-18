namespace GameObjects.Creatures;

public class CobsWall : Creature
{
    public CobsWall(int line, Player owner) : base(line, owner, LandType.CornFields, 1, 4, 2)
    {
    }

    public override void ExecuteSkill()
    {
        var contFields = Owner.ControlledLands(LandType.CornFields) +
                         Owner.Opponent.ControlledLands(LandType.CornFields);
        //todo: спешл действие
    }
}