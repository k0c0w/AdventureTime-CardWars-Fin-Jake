namespace ConsoleClient;

public class ClientHandle
{
    public static void MakeHandshake(Packet packet)
    {
        var message = packet.ReadString();
        var id = packet.ReadInt();
        Console.WriteLine(message);
        Client.Instance.Id = id;
        ClientSend.WelcomeReceived();
    }
}