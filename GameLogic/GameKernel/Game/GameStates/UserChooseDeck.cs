using Shared.GameActions;
using Shared.PossibleCards;

namespace GameKernel.GameStates;

public class UserChooseDeck : IGameState
{
    public UserChooseDeck(Game game) => CurrentGame = game;
    
    public Game CurrentGame { get; }
    public bool IsValidAction(GameAction action)
    {
        if (action is not UserChoseDeck choose) return false;
        var allowed = CurrentGame.AllowedDecks;

        return CurrentGame.Players.ContainsKey(choose.UserId)
               && (allowed.Item1 == choose.DeckType || allowed.Item2 == choose.DeckType)
               && CurrentGame._holder.ContainsDeck(choose.DeckType);
    }

    
    public void Execute(GameAction action)
    {
        var choose = (UserChoseDeck)action;
        var dnl = CurrentGame._holder.GrabDeck(choose.DeckType);
        var player = CurrentGame.Players[choose.UserId];
        player.Deck = dnl.Item1;
        player.Lands = dnl.Item2;
        CurrentGame.PlayersDeck[choose.UserId] = player.Deck;
        
        var lands = CurrentGame.Players[choose.UserId].Lands.Select(X => X.LandType).ToArray();
        CurrentGame.RegisterAction(new UserChoseDeck(choose.UserId, player.Deck.DeckType));
        CurrentGame.RegisterAction(new UserTakeDeck(choose.UserId, TakeFiveCards(choose.UserId), lands));

        if (CurrentGame.Players.Values.All(x => x.Deck != null))
            ChangeState();
    }

    private AllCards[] TakeFiveCards(int userId)
    {
        var deck = CurrentGame.PlayersDeck[userId];
        return Enumerable.Range(0, 5).Select(x => deck.GetCard()).ToArray();
    }

    public void ChangeState()
    {
        CurrentGame.RegisterAction(new UserDecisionStart {UserId = 1});
        CurrentGame._holder = null!;
        CurrentGame.GameState = new TakeCardsState(1, CurrentGame, true);
        CurrentGame.GameState.Execute(new GameAction() {UserId = 1});
        CurrentGame.GameState = new AfterGameStartPlayerDecision(CurrentGame);
    }
}