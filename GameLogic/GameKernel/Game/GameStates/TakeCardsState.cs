using Shared.GameActions;

namespace GameKernel.GameStates;

public class TakeCardsState : IGameState
{
    private readonly int player;

    private readonly bool _firstTime;

    public TakeCardsState(int player, Game game, bool firstTime = false)
    {
        CurrentGame = game;
        this.player = player;
        _firstTime = firstTime;
    }
    
    public Game CurrentGame { get; }
    
    public bool IsValidAction(GameAction action) => throw new NotImplementedException();

    public void Execute(GameAction action)
    {
        //todo: выдавать карты другим если необхоимо
        var deck = CurrentGame.PlayersDeck[action.UserId];
        if (!deck.IsEmpty)
        {
            var card = deck.GetCard();
            CurrentGame.Players[action.UserId].TakeCard(card);
            CurrentGame.RegisterAction(new UserTakeCards(action.UserId, new []{card}, deck.CardsLeft));
        }
        
        CurrentGame.Players[action.UserId].ResetEnergy();
        
        if (!_firstTime)
        {
            ChangeState();
        }
    }

    public void ChangeState()
    {
        CurrentGame.RegisterAction(new UserDecisionStart {UserId = player});
        CurrentGame.GameState = new PlayerDecision(player, CurrentGame);
    }
}