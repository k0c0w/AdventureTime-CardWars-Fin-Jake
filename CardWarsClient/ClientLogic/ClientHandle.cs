using CardWarsClient.Models;
using CardWarsClient.ViewModels;
using Microsoft.UI.Xaml.Media.Animation;
using Shared.Decks;
using Shared.GameActions;
using Shared.Packets;

namespace CardWarsClient;

public class ClientHandle
{
    public static void MakeHandshake(Packet packet)
    {
        var id = packet.ReadInt();
        var message = packet.ReadString();
        Console.WriteLine(message);
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
        if(action is UserTakeDeck deck)
        {
            Handle(deck);
        }
        else
            Handle((dynamic)action);
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

    private static void Handle(UserTakeDeck deckInfo)
    {
        var instance = GamePageViewModel.Instance;
        if (Client.Instance.Id == deckInfo.UserId)
        {
            instance.Player.TakeLands(deckInfo.Lands);
            instance.Player.TakeInitialHand(deckInfo.CardsFromDeck);
        }
        else
            instance.Opponent.TakeLands(deckInfo.Lands);
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