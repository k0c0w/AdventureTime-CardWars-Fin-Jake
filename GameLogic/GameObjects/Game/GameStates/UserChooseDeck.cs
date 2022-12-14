using GameObjects;

namespace GameKernel.GameStates;

public class UserChooseDeck : IGameState
{
    public Game CurrentGame { get; }
    public bool IsValidAction(GameAction action)
    {
        if (action is not UserDeckChoose choose) return false;
        var allowed = CurrentGame.AllowedDecks;
        
        return (allowed.Item1 == choose.Deck || allowed.Item2 == choose.Deck) 
               && CurrentGame._holder.ContainsDeck(choose.Deck) && !CurrentGame.Players.ContainsKey(choose.UserId);
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

        var player = new Player(choose.UserId, CurrentGame, dnl.Item2, dnl.Item1);
        CurrentGame.RegisterPlayer(player);
        CurrentGame.RegisterAction(new UserChoseDeck(player.Id, player.Deck.DeckType));

        if (CurrentGame.Players.Values.All(x => x != null))
            ChangeState();
    }

    public void ChangeState()
    {
        CurrentGame.RegisterAction(new GameStart());
        CurrentGame.GameState = new AfterGameStartPlayerDecision(CurrentGame);
    }
}