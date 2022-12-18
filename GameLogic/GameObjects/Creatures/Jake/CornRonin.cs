using Shared.PossibleCards;

namespace GameObjects.Creatures;

public class CornRonin : Creature
{
    public CornRonin(int line, Player owner) : base(line, owner, LandType.corn_fields, 1, 6, 1)
    { }

    public override void ExecuteSkill() => RegisterBonus(new Bonus {AttackBonus = CountNeighboringCornfields()});

    private int CountNeighboringCornfields()
    {
        var count = 0;
        if (Line != 0 && !Owner.Lands[Line - 1].IsTurnedOver && Owner.Lands[Line - 1].LandType == LandType.corn_fields)
            ++count;
        if (Line != 3 && !Owner.Lands[Line + 1].IsTurnedOver && Owner.Lands[Line + 1].LandType == LandType.corn_fields)
            ++count;
        return count;
    }
}