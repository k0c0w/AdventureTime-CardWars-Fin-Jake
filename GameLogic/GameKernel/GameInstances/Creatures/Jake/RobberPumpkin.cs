using Shared.PossibleCards;

namespace GameObjects.Creatures;

public class RobberPumpkin : Creature, IFloopable
{
    private bool _isFlupped;
    
    public RobberPumpkin(int line, Player owner, LandType land) 
        : base(line, owner, land, AllCards.robber_pumpkin,1, 5, 0)
    {
        //ReplaceCreature();
    }

    public override void ExecuteSkill()
    {
        if (!_isFlupped) return;
        
        //todo: чеее делать как? как выбрать указзанные существа?
        var controlledFields = Owner.ControlledLands(LandType.corn_fields);
        var opponentCreatures = Owner.Opponent.Creatures;
        for (var i = 0; i < controlledFields; i++)
            opponentCreatures[i]?.TakeDamage(1);
    }

    public bool CanBeFlooped() => true;

    public void Floop()
    {
        if (!CanBeFlooped()) return;
        _isFlupped = !_isFlupped;
    }

    public bool IsFlooped() => _isFlupped;
}