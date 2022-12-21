using Shared.GameActions;

namespace GameKernel.GameStates;

public class AfterGameStartPlayerDecision : PlayerDecision, IGameState
{
    public AfterGameStartPlayerDecision(Game game) : base(1, game) { }


    public new bool IsValidAction(GameAction action)
        => IsValidUserId(action) && (action is UserPutCard put && IsValidPutCardAction(put) || action is UserDecisionEnd);

    public new void Execute(GameAction action)
    {
        if (action is UserDecisionEnd)
        {
            ChangeState();
            return;
        }
        
        var put = (UserPutCard)action;
        if(CurrentGame.TryPlayCreature(CurrentGame.Players[1], put.Card, put.Line))
            CurrentGame.RegisterAction(put with{ EnergyLeft = CurrentGame.Players[1].EnergyLeft});
        else
            CurrentGame.RegisterAction(Game.BadRequestAction);
    }
    
    public new void ChangeState()
    {
        CurrentGame.GameState = new TakeCardsState(2, CurrentGame);
        CurrentGame.GameState.Execute(new GameAction() {UserId = 2});
    }
}