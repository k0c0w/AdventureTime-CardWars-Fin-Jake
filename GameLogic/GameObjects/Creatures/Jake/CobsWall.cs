using Shared.PossibleCards;

namespace GameObjects.Creatures;

public class CobsWall : Creature
{
    public CobsWall(int line, Player owner) : base(line, owner, LandType.corn_fields, 1, 4, 2)
    {
    }

    public override void ExecuteSkill()
    {
        var contFields = Owner.ControlledLands(LandType.corn_fields) +
                         Owner.Opponent.ControlledLands(LandType.corn_fields);
        //todo: спешл действие
    }
}