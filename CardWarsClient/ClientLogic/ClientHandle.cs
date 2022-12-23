using CardWarsClient.Models;
using CardWarsClient.ViewModels;
using Shared.Decks;
using Shared.GameActions;
using Shared.Packets;
using System;
using static System.Collections.Specialized.BitVector32;

namespace CardWarsClient;

public class ClientHandle
{
    private static GamePageViewModel ViewModel = GamePageViewModel.Instance;

    public static void MakeHandshake(Packet packet)
    {
        var id = packet.ReadInt();
        Client.Instance.Id = id;
        ClientSend.WelcomeReceived();
    }

    public static void SendToGame(Packet packet)
    {
       
        //Client.Instance.IsServerReady = true;
    }

    public static void Dispatch(Packet packet)
    {
        try
        {
            var action = PacketEncoder.DecodeGameAction(packet);
            Handle((dynamic)action);
        }
        catch(Exception ex) 
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Shell.Current.DisplayAlert("ошибка", ex.GetType().ToString(), "jr");
            });
        }
        
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
            ViewModel.Player.Hp -= damage.Damage;
        else
            ViewModel.Opponent.Hp -= damage.Damage;
    }

    private static void Handle(UserTakeLands deckInfo)
    {

        if (Client.Instance.Id == deckInfo.UserId)
            ViewModel.Player.TakeLands(deckInfo.Lands);
        else
            ViewModel.Opponent.TakeLands(deckInfo.Lands);
    }

    private static void Handle(UserTakeCards takenCards)
    {
        if (Client.Instance.Id == takenCards.UserId)
            ViewModel.Player.TakeCards(takenCards.Cards);
    }

    private static void Handle(UserPutCard putCard)
    {
        if (Client.Instance.Id == putCard.UserId)
        {   
            ViewModel.Player.Lands[putCard.Line].BindedCard = new CardModel { Name = putCard.Card };
            ViewModel.Player.Hand.Remove(putCard.Card);
            ViewModel.ActionsCount = putCard.EnergyLeft;
            ViewModel.AvailableActionsPrompt = $"Доступные действия: {putCard.EnergyLeft}";
        }     
        else ViewModel.Opponent.Lands[putCard.Line].BindedCard = new CardModel { Name = putCard.Card };
    }

    private static void Handle(UserDecisionStart decisionStart)
    {
        if (Client.Instance.Id == decisionStart.UserId)
        {
            ViewModel.IsCurrentPlayerTurn = true;
            ViewModel.ActionsCount = 2;
            ViewModel.AvailableActionsPrompt = "Доступные действия: 2";
            MainThread.BeginInvokeOnMainThread(async () =>
            {
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

        if (chose.UserId == Client.Instance.Id)
            SetDeck(ViewModel.Player, chose.DeckType);
        else
            SetDeck(ViewModel.Opponent, chose.DeckType);
    }

    private static void Handle(GameStart info)
    {
        if (info.FirstPlayerInfo.Item1 == Client.Instance.Id)
        {
            ViewModel.Opponent.Name = info.SecondPlayerInfo.Item2;
        }
        else
        {
            ViewModel.Opponent.Name = info.FirstPlayerInfo.Item2;
        }

        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await Shell.Current.GoToAsync("GamePage");
        });
    }
    private static void Handle(BadRequest badRequest)
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await Shell.Current.DisplayAlert("Bad Request", "Вы сделали недопустимую операцию!", "ОK");
        });
    }

    private static void Handle(Winner winner)
    {
        if(winner.UserId == Client.Instance.Id)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Shell.Current.DisplayAlert("Победа!", "Вы победили в игре!", "Выйти");
                Application.Current.Quit();
            });
        } else if(winner.UserId == -1)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Shell.Current.DisplayAlert("Ничья!", "У обоих игроков закончилось здоровье!", "Выйти");
                Application.Current.Quit();
            });
        } else
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Shell.Current.DisplayAlert("Поражение...!", "Вы проиграли!", "Выйти");
                Application.Current.Quit();
            });
        }
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