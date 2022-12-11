namespace ConsoleClient;

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
}