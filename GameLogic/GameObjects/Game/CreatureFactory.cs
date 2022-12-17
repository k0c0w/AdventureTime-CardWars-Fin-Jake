using GameObjects;
using GameObjects.Creatures;
using Shared.PossibleCards;

namespace GameKernel;

public static class CreatureFactory
{
    public static Creature Summon(Player owner, AllCards card, int toLine)
    {
        return card switch
        {
            AllCards.ArcherDan => new ArcherDan(toLine, owner),
            AllCards.CobsLegion => new CobsLegion(toLine, owner),
            _ => throw new InvalidOperationException("Can not summon that card"),
        };
    }
}