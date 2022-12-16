namespace GameObjects.Creatures;

public class RobberPumpkin : Creature, IFlupable
{
    private bool _isFlupped;
    
    public RobberPumpkin(int line, Player owner, LandType land) : base(line, owner, land, 1, 5, 0)
    {
        //ReplaceCreature();
    }

    public override void ExecuteSkill()
    {
        if (!_isFlupped) return;
        
        //todo: чеее делать как? как выбрать указзанные существа?
        var controlledFields = Owner.ControlledLands(LandType.CornFields);
        var opponentCreatures = Owner.Opponent.Creatures;
        for (var i = 0; i < controlledFields; i++)
            opponentCreatures[i]?.TakeDamage(1);
    }

    public bool CanBeFlupped() => true;

    public void Flup()
    {
        if (!CanBeFlupped()) return;
        _isFlupped = !_isFlupped;
    }

    public bool IsFlupped() => _isFlupped;
}