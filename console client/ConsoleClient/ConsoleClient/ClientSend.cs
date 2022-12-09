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
        using var packet = new Packet((int)ClientPackets.WelcomeReceived);
        packet.Write(Client.Instance.Id);
        //todo: additional info ex Username
        
        SendTCPData(packet);
    }
}