using Shared.PossibleCards;

namespace GameObjects.Creatures;

public class ArcherDan : Creature, IFloopable
{
    public ArcherDan(int line, Player owner) 
        : base(line, owner, LandType.corn_fields, AllCards.archer_dan,2, 6, 2)
    { }

    public override void ExecuteSkill()
    {
        if(IsFlooped) return;
        Owner.Opponent.Buildings[Line]?.Destroy();
    }

    public bool CanBeFlooped() => !IsFlooped;

    public void Floop()
    {
        if (!CanBeFlooped())
            return;
        Owner.Opponent.Buildings[Line]?.Destroy();
        //todo: Owner.CurrentGame.RegisterAction(new CardFloop());
        IsFlooped = true;
    }
}