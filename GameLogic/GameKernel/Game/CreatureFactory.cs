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
            AllCards.corn_knight => new CornKnight(toLine, owner),
            AllCards.king_of_fields => new KingOfFields(toLine, owner),
            AllCards.corn_wall => new CornWall(toLine, owner),
            AllCards.corn_dog => new CornDog(toLine, owner),
            
            AllCards.spirit_solder => new SpiritSolder(toLine, owner),
            AllCards.cool_dog => new CoolDog(toLine, owner),
            AllCards.woadic_chief => new WoadicChief(toLine, owner),
            
            AllCards.nice_ice_baby => new NiceIceBaby(toLine, owner),
            AllCards.pig => new Pig(toLine, owner),
            _ => throw new InvalidOperationException("Can not summon that card"),
        };
    }
}