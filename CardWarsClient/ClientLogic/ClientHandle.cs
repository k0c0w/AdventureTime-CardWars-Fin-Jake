using CardWarsClient.Models;
using CardWarsClient.ViewModels;
using Microsoft.UI.Xaml.Media.Animation;
using Shared.Decks;
using Shared.GameActions;
using Shared.Packets;
using System;

namespace CardWarsClient;

public class ClientHandle
{
    public static void MakeHandshake(Packet packet)
    {
        var id = packet.ReadInt();
        Client.Instance.Id = id;
        ClientSend.WelcomeReceived();
    }

    public static void SendToGame(Packet packet)
    {
        packet.ReadInt();
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await Shell.Current.GoToAsync("GamePage");
        });
        //Client.Instance.IsServerReady = true;
    }

    public static void Dispatch(Packet packet)
    {
        var action = PacketEncoder.DecodeGameAction(packet);
        if (action is UserTakeCards e)
            Handle(e);
        else Handle((dynamic)action);
    }

    private static void Handle(PossibleDecks decks)
    {
        MainThread.BeginInvokeOnMainThread(async () => {
            var action = await Shell.Current.DisplayActionSheet("decks", "Отмена", "Удалить", decks.First.ToString(), decks.Second.ToString());
            if (action != null)
            {
                var chosen = (DeckTypes)Enum.Parse(typeof(DeckTypes), action);
                ClientSend.ChooseDeck(chosen);
            }
            else
            {
                Handle(decks);
            }
        });
    }

    private static void Handle(UserTakeDamage damage)
    {
        if (Client.Instance.Id == damage.UserId)
            GamePageViewModel.Instance.Player.Hp -= damage.Damage;
        else
            GamePageViewModel.Instance.Opponent.Hp -= damage.Damage;
    }

    private static void Handle(UserTakeLands deckInfo)
    {

        if (Client.Instance.Id == deckInfo.UserId)
            GamePageViewModel.Instance.Player.TakeLands(deckInfo.Lands);
        else
            GamePageViewModel.Instance.Opponent.TakeLands(deckInfo.Lands);
    }

    private static void Handle(UserTakeCards takenCards)
    {
        if (Client.Instance.Id == takenCards.UserId)
            GamePageViewModel.Instance.Player.TakeCards(takenCards.Cards);
    }

    private static void Handle(UserPutCard putCard)
    {
        if (Client.Instance.Id == putCard.UserId)
        {
            GamePageViewModel.Instance.Player.Lands[putCard.Line].BindedCard = new CardModel { Name = putCard.Card };
            GamePageViewModel.Instance.Player.Hand.Remove(putCard.Card);
            GamePageViewModel.Instance.ActionsCount = putCard.EnergyLeft;
            GamePageViewModel.Instance.AvailableActionsPrompt = $"Доступные действия: {putCard.EnergyLeft}";
        }     
        else GamePageViewModel.Instance.Opponent.Lands[putCard.Line].BindedCard = new CardModel { Name = putCard.Card };
    }

    private static void Handle(UserDecisionStart decisionStart)
    {
        if (Client.Instance.Id == decisionStart.UserId)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                GamePageViewModel.Instance.IsCurrentPlayerTurn = true;
                GamePageViewModel.Instance.ActionsCount = 2;
                GamePageViewModel.Instance.AvailableActionsPrompt = "Доступные действия: 2";
                await Shell.Current.DisplayAlert("Уведомление", "Ваш ход!", "ОK");       
            });
        } else
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                GamePageViewModel.Instance.IsCurrentPlayerTurn = false;
                await Shell.Current.DisplayAlert("Уведомление", "Дождитесь хода соперника!", "ОK");           
            });
        }
    }

    private static void Handle(CreatureState state)
    {
        if(Client.Instance.Id == state.Owner)
        {
            GamePageViewModel.Instance.Player.Lands[state.Line].BindedCard.TakenDamage += state.HPBefore - state.HPAfter;
            GamePageViewModel.Instance.Player.Lands[state.Line].BindedCard.HasDamage = true;
            if (state.IsDead)
            {
                GamePageViewModel.Instance.Player.Lands[state.Line].BindedCard = null;
            }
        }
        else
        {
            GamePageViewModel.Instance.Opponent.Lands[state.Line].BindedCard.TakenDamage += state.HPBefore - state.HPAfter;
            GamePageViewModel.Instance.Opponent.Lands[state.Line].BindedCard.HasDamage = true;
            if (state.IsDead)
            {
                GamePageViewModel.Instance.Opponent.Lands[state.Line].BindedCard = null;
            }
        }
    }

    private static void Handle(UserChoseDeck chose)
    {

        var instance = GamePageViewModel.Instance;
        if (chose.UserId == Client.Instance.Id)
            SetDeck(instance.Player, chose.DeckType);
        else
            SetDeck(instance.Opponent, chose.DeckType);
    }

    private static void Handle(GameAction action)
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await Shell.Current.DisplayAlert("Уведомление", action.ToString(), "ОK");
        });
    }

    private static void SetDeck(PlayerModel player, DeckTypes deck)
    {
        player.Deck = deck;
    }
}