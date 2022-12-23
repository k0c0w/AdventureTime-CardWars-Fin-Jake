using Shared.PossibleCards;

namespace GameObjects.Creatures;

public class CornWall : Creature
{
    public CornWall(int line, Player owner) : base(line, owner, LandType.corn_fields, AllCards.corn_wall,1, 4, 2)
    {
    }

    public override void ExecuteSkill()
    {
        var contFields = Owner.ControlledLands(LandType.corn_fields) +
                         Owner.Opponent.ControlledLands(LandType.corn_fields);
        RegisterBonus(new Bonus {DefenceBonus = contFields});
    }
}