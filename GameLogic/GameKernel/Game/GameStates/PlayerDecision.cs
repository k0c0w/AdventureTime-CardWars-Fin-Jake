using Shared.GameActions;
using GameObjects;

namespace GameKernel.GameStates;

public class PlayerDecision : IGameState
{
    public PlayerDecision(int playerId, Game game)
    {
        CurrentGame = game;
        _playerId = playerId;
    }

    private readonly int _playerId;
    
    public Game CurrentGame { get; }

    protected bool IsValidUserId(GameAction action) => action.UserId == _playerId;

    protected bool IsValidPutCardAction(UserPutCard put)
    {
        return PlayerHasCardInHand(put) && IsValidLine(put);
    }

    private bool IsValidFlupAction(UserFlupCard floop)
    {
        return IsCardOnLine(floop) && floop is IFloopable floopable && floopable.CanBeFlooped();
    }

    private bool IsCardOnLine(CardAction action)
    {
        //todo: добавить остальные типы карт
        return action is UserFlupCard flup &&
               CurrentGame.PlayersCreatures[flup.UserId][flup.Line] != null;
    }

    private static bool IsValidLine(UserPutCard putCard) => 0 <= putCard.Line & putCard.Line < 4;
    
    private bool PlayerHasCardInHand(UserPutCard putCard)
    {
        return CurrentGame.Players[putCard.UserId].HasCardInHand(putCard.IndexInHand, putCard.Card);
    }

    public bool IsValidAction(GameAction action)
    {
        if (!IsValidUserId(action)) return false;
        else if (action is UserDecisionEnd) return true;
        return ((action is UserPutCard put && IsValidPutCardAction(put))
                || action is UserFlupCard flup && IsValidFlupAction(flup));
    }

    public void Execute(GameAction action)
    {
        if (action is UserDecisionEnd)
        {
            ChangeState();
            return;
        }

        if (action is UserPutCard put 
            && CurrentGame.TryPlayCreature(CurrentGame.Players[put.UserId], put.IndexInHand, put.IndexInHand))
        {
            CurrentGame.RegisterAction(put);
        }
        else
        {
            CurrentGame.RegisterAction(Game.BadRequestAction);
        }
        //todo: logic
    }

    public void ChangeState()
    {
        CurrentGame.GameState = new AttackState(_playerId, CurrentGame);
        CurrentGame.GameState.Execute(null!);
    }
}