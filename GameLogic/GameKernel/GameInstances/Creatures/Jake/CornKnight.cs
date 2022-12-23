using Shared.PossibleCards;

namespace GameObjects.Creatures;

public class CornKnight : Creature
{
    public CornKnight(int line, Player owner) 
        : base(line, owner, LandType.corn_fields, AllCards.corn_knight, 1, 0, 0)
    { }

    public override void ExecuteSkill()
    {
        var controlledCornFields = Owner.ControlledLands(LandType);
        RegisterBonus(new Bonus(2 * controlledCornFields, controlledCornFields));
    }
}