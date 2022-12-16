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
    
    protected int EnergyLeft = 2;

    
    public Game CurrentGame { get; }

    protected bool IsValidUserId(GameAction action) => action.UserId == _playerId;

    protected bool IsValidPutCardAction(UserPutCard put)
    {
        return PlayerHasCardInHand(put) && IsValidLine(put);
    }

    private bool IsValidFlupAction(UserFlupCard flup)
    {
        return IsCardOnLine(flup) && flup is IFlupable flupable && flupable.CanBeFlupped();
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
        var playerHand = CurrentGame.PlayersHand[1]; 
        return 0 <= putCard?.IndexInHand && putCard?.IndexInHand < playerHand.Count
                                         && playerHand[(int)putCard.IndexInHand] == putCard.Card;
    }

    public bool IsValidAction(GameAction action)
    {
        return action.UserId == _playerId 
               && ((action is UserPutCard put && IsValidPutCardAction(put))
                   || action is UserFlupCard flup && IsValidFlupAction(flup));
    }

    public void Execute(GameAction action)
    {
        if (action.UserId == _playerId && action is UserDecisionEnd)
        {
            ChangeState();
            return;
        }

        if (!IsValidAction(action))
        {
            CurrentGame.RegisterAction(Game.BadRequestAction);
        }


        //todo: logic
    }

    public void ChangeState()
    {
        throw new NotImplementedException();
    }
}