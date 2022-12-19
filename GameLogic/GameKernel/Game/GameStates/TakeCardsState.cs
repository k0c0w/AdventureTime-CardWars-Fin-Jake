using Shared.GameActions;

namespace GameKernel.GameStates;

public class TakeCardsState : IGameState
{
    private readonly int nextPlayer;

    public TakeCardsState(int nextPlayer, Game game)
    {
        CurrentGame = game;
        this.nextPlayer = nextPlayer;
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
    }

    public void ChangeState()
    {
        throw new NotImplementedException();
    }
}