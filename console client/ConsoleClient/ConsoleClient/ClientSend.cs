namespace ConsoleClient;

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
        packet.Write("fdfsf");
        SendTCPData(packet);
    }
}