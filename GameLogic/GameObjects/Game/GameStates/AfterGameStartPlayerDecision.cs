using Shared.GameActions;

namespace GameKernel.GameStates;

public class AfterGameStartPlayerDecision : PlayerDecision, IGameState
{
    public AfterGameStartPlayerDecision(Game game) : base(1, game) { }


    public new bool IsValidAction(GameAction action)
        => IsValidUserId(action) && action is UserPutCard put && IsValidPutCardAction(put);

    public new void Execute(GameAction action)
    {
        if (!IsValidAction(action))
        {
            CurrentGame.RegisterAction(Game.BadRequestAction);
            return;
        }

        var put = (UserPutCard)action;
        if(CurrentGame.TryPlayCard(CurrentGame.Players[1], (int)put.IndexInHand!, put.Line))
        {
            EnergyLeft -= CurrentGame.PlayersCreatures[1][put.Line]!.SummonCost;
            CurrentGame.RegisterAction(put);
        }
        else
            CurrentGame.RegisterAction(Game.BadRequestAction);
        if(EnergyLeft <= 0)
            ChangeState();
    }
    
    public new void ChangeState()
    {
        CurrentGame.GameState = new PlayerDecision(2, CurrentGame);
    }
}