using Shared.PossibleCards;

namespace GameObjects.Creatures;

public class CornDog : Creature
{
    public CornDog(int line, Player owner) : base(line, owner, LandType.corn_fields, AllCards.corn_dog, 1, 12, 0)
    {
    }

    public override void ExecuteSkill()
    {
        var damage = 0;
        var defence = Owner.ControlledLands(LandType.corn_fields);
        if (defence <= 3)
            damage = 1;
        RegisterBonus(new Bonus(defence, damage));
    }
}