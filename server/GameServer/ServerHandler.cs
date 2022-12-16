using GameKernel;
using GameKernel.Deck;

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
        Server.Clients[fromClient].Player = player;
        Console.WriteLine($"{Server.Clients[fromClient].Tcp.Socket.Client.RemoteEndPoint} has successfully connected and is now player {fromClient} (nickname: {username})");

        if(fromClient != clientIdCheck)
            Console.WriteLine($"Player {username}(ID: {fromClient} is accusing to have wrong Client ID {clientIdCheck}");
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
        lock (Server.Clients)
        {
            if (Server.Clients.Values.Any(x => x.Player == null || !x.Player.IsReady))
                return;
        }

        Server.CurrentGame = new Game(new FinnVSJake(), 1, 2);
        using var decks = GetEncodedDecks(Server.CurrentGame);
        using var packet = new Packet((int)PacketId.GameActionPacket, (int)GameActionPacket.GameStart);
        //todo: убрать это
        packet.Write("test");
        ServerSend.SendTCPDataToAll(packet);
        ServerSend.SendTCPDataToAll(decks);
    }

    private static Packet GetEncodedDecks(Game gameIsInStartState)
    {
        var decks = gameIsInStartState.ApplyGameActions(new GameStart());
        return PacketEncoder.EncodeGameAction(decks.First());
    }
}