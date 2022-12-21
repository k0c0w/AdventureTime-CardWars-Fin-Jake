using Shared.Packets;

namespace CardWarsClient;

public class ClientSend
{

    private static void SendTCPData(Packet packet)
    {
        packet.WriteLength();
        Client.Instance.Tcp.SendData(packet);
    }

    public static void WelcomeReceived()
    {
        using var packet = new Packet((int)PacketId.ClientPacket);
        packet.Write((int)ClientPacket.WelcomeReceived);
        packet.Write(Client.Instance.Id);
        //todo: additional info ex Username
        packet.Write("name" + Client.Instance.Id);
        SendTCPData(packet);
    }

    public static void ReadyChange(bool isReady)
    {
        using var packet = new Packet((int)PacketId.ClientPacket);
        packet.Write((int)ClientPacket.ReadyForGame);
        packet.Write(isReady);
        SendTCPData(packet);
    }

    public static void ChooseDeck(Shared.Decks.DeckTypes deck)
    {
        var action = new Shared.GameActions.UserChoseDeck(Client.Instance.Id, deck);

        var r = PacketEncoder.EncodeGameAction(action);
        SendTCPData(r);
    }

    public static void PutCard(Shared.PossibleCards.AllCards card, int line)
    {
        var action = new Shared.GameActions.UserPutCard(Client.Instance.Id, line, card);

        var r = PacketEncoder.EncodeGameAction(action);
        SendTCPData(r);
    }

    public static void EndTurn()
    {
        var action = new Shared.GameActions.UserDecisionEnd();

        var r = PacketEncoder.EncodeGameAction(action);
        SendTCPData(r);
    }
}