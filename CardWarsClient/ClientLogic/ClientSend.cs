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
}