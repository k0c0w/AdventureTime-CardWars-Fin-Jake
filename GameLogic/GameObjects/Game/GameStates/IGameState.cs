using Shared.GameActions;

namespace GameKernel.GameStates;

public interface IGameState
{
    Game CurrentGame { get; }
    
    bool IsValidAction(GameAction action);
    
    void Execute(GameAction action);

    void ChangeState();
}