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
            AllCards.CornRonin => new CornRonin(toLine, owner),
            AllCards.SpiritSolder => new SpiritSolder(toLine, owner),
            _ => throw new InvalidOperationException("Can not summon that card"),
        };
    }
}