using GameObjects;

namespace GameKernel.GameStates;

public class UserChooseDeck : IGameState
{
    public UserChooseDeck(Game game) => CurrentGame = game;
    
    public Game CurrentGame { get; }
    public bool IsValidAction(GameAction action)
    {
        if (action is not UserDeckChoose choose) return false;
        var allowed = CurrentGame.AllowedDecks;

        return CurrentGame.Players.ContainsKey(choose.UserId)
               && (allowed.Item1 == choose.Deck || allowed.Item2 == choose.Deck)
               && CurrentGame._holder.ContainsDeck(choose.Deck);
    }

    
    public void Execute(GameAction action)
    {
        if (!IsValidAction(action))
        {
            CurrentGame.RegisterAction(new BadRequest());
            return;
        }

        var choose = (UserDeckChoose)action;
        var dnl = CurrentGame._holder.GrabDeck(choose.Deck);
        var player = CurrentGame.Players[choose.UserId];
        player.Deck = dnl.Item1;
        player.Lands = dnl.Item2;
        
        CurrentGame.RegisterAction(new UserChoseDeck(player.Id, player.Deck.DeckType));

        if (CurrentGame.Players.Values.All(x => x.Deck != null))
            ChangeState();
    }

    public void ChangeState()
    {
        CurrentGame.RegisterAction(new GameStart());
        CurrentGame._holder = null!;
        CurrentGame.GameState = new AfterGameStartPlayerDecision(CurrentGame);
    }
}