using Shared.PossibleCards;

namespace GameObjects.Creatures;

public class KingOfFields : Creature
{

    public KingOfFields(int line, Player owner) 
        : base(line, owner, LandType.corn_fields, AllCards.king_of_fields, 1, 7, 0)
    { }

    public override void ExecuteSkill() 
        => RegisterBonus(new Bonus { AttackBonus = Owner.Creatures.Count(x => x?.LandType == LandType.corn_fields) - 1});
}