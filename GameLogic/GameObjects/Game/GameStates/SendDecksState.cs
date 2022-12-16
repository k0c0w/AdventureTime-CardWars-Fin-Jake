using GameKernel.Deck;

namespace GameKernel.GameStates;

public class DeckChooseGameState : IGameState
{
    public Game CurrentGame { get; }

    public DeckChooseGameState(Game game) => CurrentGame = game;

    public bool IsValidAction(GameAction action) => false;

    public void Execute(GameAction action)
    {
        CurrentGame.RegisterAction(new PossibleDecks{UserId = -1, First = DeckTypes.FinnDeck, Second = DeckTypes.JakeDeck});
        ChangeState();
    }

    public void ChangeState()
    {
        CurrentGame.GameState = new UserChooseDeck(CurrentGame);
    }
}