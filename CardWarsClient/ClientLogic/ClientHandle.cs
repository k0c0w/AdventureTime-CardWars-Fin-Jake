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
        Handle((dynamic)action);
    }

    private static void Handle(PossibleDecks decks)
    {
        MainThread.BeginInvokeOnMainThread(async () => {
            var action = await Shell.Current.DisplayActionSheet("decks", "Отмена", "Удалить", decks.First.ToString(), decks.Second.ToString());
            var chosen = (DeckTypes)Enum.Parse(typeof(Shared.Decks.DeckTypes), action);
            ClientSend.ChooseDeck(chosen);
        }) ;
    }

    private static void Handle(GameAction action)
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await Shell.Current.DisplayAlert("Уведомление", action.ToString(), "ОK");
        });
    }
}