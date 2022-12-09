namespace GameServer;
public class ServerHandler
{
    public static void WelcomeReceived(int fromClient, Packet packet)
    {
        var clientIdCheck = packet.ReadInt();
        var username = packet.ReadString();
        Console.WriteLine($"{Server.Clients[fromClient].Tcp.socket.Client.RemoteEndPoint} has successfully connected and is now player {fromClient} (nickname: {username})");

        if(fromClient != clientIdCheck)
            Console.WriteLine($"Player {username}(ID: {fromClient} is accusing to have wrong Client ID {clientIdCheck}");
    }
}