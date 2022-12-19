using Shared.Decks;
using Shared.GameActions;
using Shared.PossibleCards;

namespace GameKernelTests;

public class Tests
{
    public Game game;
    
    
    [SetUp]
    public void Setup()
    {
        game = new Game(new FinnVSJake(), 1, 2);
    }

    [Test]
    public void SendsCorrectDeck()
    {
        var response = Init();
        
        Assert.AreEqual(new PossibleDecks {UserId = -1, First = DeckTypes.FinnDeck, Second = DeckTypes.JakeDeck},
            response[0]);
    }

    [Test]
    public void ChooseDeckAndGrab()
    {
        var response = ReceiveDeck();
        
        Assert.AreEqual(2, response.Count);
        Assert.AreEqual(new UserChoseDeck(1, DeckTypes.FinnDeck), response[0]);
        Assert.IsTrue(response[1] is UserTakeDeck);
        var deck = response[1] as UserTakeDeck; 
        Assert.AreEqual(4, deck?.Lands.Length);
        Assert.AreEqual(5, deck?.CardsFromDeck.Length);
    }

    [Test]
    public void SecondUserChose()
    {
        var response = GameStart();
        
        Assert.IsTrue(response.Contains(new GameStart()));
    }

    [Test]
    public void UserPutCardWhenUserChoseState()
    {
        ReceiveDeck();
        
        var response = SendAction(new UserPutCard(1, 1, AllCards.corn_ronin, 0));
        
        Assert.IsTrue(response.Count == 1);
        Assert.AreEqual(response[0], new BadRequest());
    }

    [Test]
    public void UserPlayCard()
    {
        GameStart();
        var action = new UserPutCard(1, 0, AllCards.spirit_solder, 0);
        var response = SendAction(action);
        Assert.IsTrue(response.Count == 1);
        Assert.AreEqual(response[0], action);
    }

    [Test]
    public void UserPlayedCardNotInHisTurn()
    {
        GameStart();
        var action = new UserPutCard(2, 0, AllCards.spirit_solder, 0);
        var response = SendAction(action);
        Assert.IsTrue(response.Count == 1);
        Assert.AreEqual(response[0], new BadRequest());
    }

    private List<GameAction> ReceiveDeck()
    {
        Init();
        return SendAction(new UserChoseDeck(1, DeckTypes.FinnDeck));
    }

    private List<GameAction> GameStart()
    {
        ReceiveDeck();
        return SendAction(new UserChoseDeck(2, DeckTypes.JakeDeck));
    }


    private List<GameAction> Init()
    {
        return SendAction(new GameStart());
    }
    
    private List<GameAction> SendAction(GameAction action) => game.ApplyGameActions(action).ToList();

}