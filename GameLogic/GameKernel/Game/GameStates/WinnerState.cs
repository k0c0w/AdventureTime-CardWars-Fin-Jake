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
        CurrentGame.IsFinished = true;
        if (CurrentGame.Players.Values.All(x => x.IsDead))
        {
            CurrentGame.RegisterAction(new Winner(-1));
            return;
        }
        var winner = CurrentGame.Players.Values.First(x => !x.IsDead);
        CurrentGame.RegisterAction(new Winner(winner.Id));
    }

    public void ChangeState()
    {
        throw new NotImplementedException();
    }
}