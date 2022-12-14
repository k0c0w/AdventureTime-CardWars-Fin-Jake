namespace GameKernel.GameStates;

public class AfterGameStartPlayerDecision : IGameState
{
    public AfterGameStartPlayerDecision(Game game) => CurrentGame = game;
    
    public Game CurrentGame { get; }
    public bool IsValidAction(GameAction action)
    {
        //return action.UserId && //todo:
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