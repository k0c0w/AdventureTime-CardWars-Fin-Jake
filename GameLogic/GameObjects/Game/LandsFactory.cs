using GameKernel.temp;
using GameObjects;

namespace GameKernel;

internal class LandsFactory
{
    public static DecksAndLandsHolder PrepareLandsAndDecks(FinnVSJake setting)
    {
        var finn = new Land[4]
        {
            new Land(LandType.BluePlains), new Land(LandType.BluePlains), 
            new Land(LandType.BluePlains), new Land(LandType.BluePlains)
        };
        
        var jake = new Land[4]
        {
            new Land(LandType.CornFields), new Land(LandType.CornFields), 
            new Land(LandType.CornFields), new Land(LandType.CornFields)
        };

        var finnDeck = new temp.Deck();
        var jakeDeck = new temp.Deck();
        return new DecksAndLandsHolder
        {
            First = Tuple.Create(finnDeck, finn),
            Second = Tuple.Create(jakeDeck, jake)
        };
    }
}