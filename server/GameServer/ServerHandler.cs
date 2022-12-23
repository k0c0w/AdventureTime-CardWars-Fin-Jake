using GameKernel;
using Shared.GameActions;
using Shared.Packets;

namespace GameServer;
internal class ServerHandler
{
    private delegate void AfterReadinessChanged();
    private static event AfterReadinessChanged UserReadinessChanged;
    static ServerHandler() => UserReadinessChanged += TryStartGame; 
    
    public static void WelcomeReceived(int fromClient, Packet packet)
    {
        var clientIdCheck = packet.ReadInt();
        var username = packet.ReadString();
        var player = new Player(fromClient, username);
        SendAlreadyConnected(fromClient);
        SendUsernameToOthers(player, fromClient);

        Server.Clients[fromClient].Player = player;
        Console.WriteLine($"{Server.Clients[fromClient].Tcp.Socket.Client.RemoteEndPoint} has successfully connected and is now player {fromClient} (nickname: {username})");
        
        if(fromClient != clientIdCheck)
            Console.WriteLine($"Player {username}(ID: {fromClient} is accusing to have wrong Client ID {clientIdCheck}");
    }

    private static void SendUsernameToOthers(Player player, int except)
    {
        using var packet = GetPlayerUsername(player);
        ServerSend.SendTCPDataToAll(except, packet);
    }

    private static void SendAlreadyConnected(int toClient)
    {
        foreach (var connected in Server.Clients.Values.Where(x => x.Player != null).Select(x => x.Player))
        {
            using var packet = GetPlayerUsername(connected!);
            ServerSend.SendTCPData(toClient, packet);
        }
    }

    private static Packet GetPlayerUsername(Player player)
    {
        var packet = new Packet();
        packet.Write((int)PacketId.ServerPacket);
        packet.Write((int)ServerPacket.AlreadyConnected);
        packet.Write(player.Username.Length);
        packet.Write(player.Username);
        return packet;
    }
    
    public static void ChangeUserReadiness(int fromClient, Packet packet)
    {
        var readiness = packet.ReadBool();
        var player = Server.Clients[fromClient].Player;
        player!.IsReady = readiness;
        Task.Run(() => UserReadinessChanged());
        Console.WriteLine($"User {player.Username} is ready: {player.IsReady}");
    }
    
    private static void TryStartGame()
    {
        if (Server.Clients.Values.Any(x => x.Player is not { IsReady: true }) 
            || Server.CurrentGame is { IsFinished: false }) return;

        Server.CurrentGame = new Game(new FinnVSJake(), 1, 2);
        using var decks = GetEncodedDecks(Server.CurrentGame);
        using var packet = new Packet((int)PacketId.GameActionPacket, (int)GameActionPacket.GameStart);
        ServerSend.SendTCPDataToAll(packet);
        ServerSend.SendTCPDataToAll(decks);
        Console.WriteLine("Created new game");
    }

    private static Packet GetEncodedDecks(Game gameIsInStartState)
    {
        var decks = gameIsInStartState.ApplyGameActions(new GameStart());
        return PacketEncoder.EncodeGameAction(decks.First());
    }
}