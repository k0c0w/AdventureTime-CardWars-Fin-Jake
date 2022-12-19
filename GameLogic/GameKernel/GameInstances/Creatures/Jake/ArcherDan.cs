using Shared.PossibleCards;

namespace GameObjects.Creatures;

public class ArcherDan : Creature, IFloopable
{
    public ArcherDan(int line, Player owner) 
        : base(line, owner, LandType.corn_fields, AllCards.archer_dan,2, 6, 2)
    { }

    public override void ExecuteSkill()
    {
        if(!IsFlooped()) return;
        Owner.Opponent.Buildings[Line]?.Destroy();
    }

    public bool CanBeFlooped() => true;

    public void Floop()
    {
        if (!CanBeFlooped())
            return;
        //todo: some actions assosiated with flup
        IsAttacking = !IsAttacking;
    }

    public bool IsFlooped() => IsAttacking;
}