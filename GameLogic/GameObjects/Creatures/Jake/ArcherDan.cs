namespace GameObjects.Creatures;

public class ArcherDan : Creature, IFlupable
{
    public ArcherDan(int line, Player owner) : base(line, owner, LandType.CornFields, 2, 6, 2)
    {
    }

    public override void ExecuteSkill()
    {
        if(!IsFlupped()) return;
        Owner.Opponent.Buildings[Line]?.Destroy();
    }

    public bool CanBeFlupped() => true;

    public void Flup()
    {
        if (!CanBeFlupped())
            return;
        //todo: some actions assosiated with flup
        IsAttacking = !IsAttacking;
    }

    public bool IsFlupped() => IsAttacking;
}