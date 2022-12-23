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
            AllCards.corn_ronin => new CornRonin(toLine, owner),
            AllCards.spirit_solder => new SpiritSolder(toLine, owner),
            AllCards.cool_dog => new CoolDog(toLine, owner),
            _ => throw new InvalidOperationException("Can not summon that card"),
        };
    }
}