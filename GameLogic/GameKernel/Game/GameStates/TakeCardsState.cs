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
        var card = deck.GetCard();
        CurrentGame.Players[action.UserId].TakeCard(card);
        CurrentGame.RegisterAction(new UserTakeCard(action.UserId, card, deck.CardsLeft));
        if (!_firstTime)
        {
            ChangeState();
        }
    }

    public void ChangeState()
    {
        CurrentGame.GameState = new PlayerDecision(player, CurrentGame);
    }
}