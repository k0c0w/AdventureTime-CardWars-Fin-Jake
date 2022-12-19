using Shared.PossibleCards;

namespace GameObjects.Creatures;

public class CobsLegion : Creature
{
    public CobsLegion(int line, Player owner, bool heroComeBack=false) 
        : base(line, owner, LandType.corn_fields, 2, 8, 2)
    {
        if (heroComeBack)
        {
            //todo: подумать куда положить replace creature или везде протягивать их?
            var creature = ReplaceCreature();
            //if (creature != null)
                //Owner.Hand.Add(creature);
        }
    }

    public override void ExecuteSkill() { }
}