using Shared.GameActions;

namespace GameKernel.GameStates;

public class WinnerState : IGameState
{
    public WinnerState(Game game) => CurrentGame = game;
    
    public Game CurrentGame { get; }
    public bool IsValidAction(GameAction action)
    {
        throw new NotImplementedException();
    }

    public void Execute(GameAction action)
    {
        throw new NotImplementedException();
    }

    public void ChangeState()
    {
        throw new NotImplementedException();
    }
}