using GameKernel;
using Shared.GameActions;
using Shared.Packets;

namespace GameServer;

public class GameActionHandler
{
    public async static void ApplyToGame(int fromClient, Packet packet)
    {
        if(Server.CurrentGame == null) return;
        if (Server.CurrentGame.IsFinished)
        {
            Server.CurrentGame = null;
            return;
        }
        try
        {
            await ResponseToClientsAsync(Server.CurrentGame.ApplyGameActions(GetGameAction(fromClient, packet)), fromClient);
        }
        catch (Exception e)
        {
            ServerSend.SendTCPData(fromClient, new Packet((int)PacketId.ServerPacket, (int)ServerPacket.ServerError));
            Console.WriteLine(e);
        }
    }
    
    private static GameAction GetGameAction(int user, Packet packet)
    {
        var gameAction = PacketEncoder.DecodeGameAction(packet);
        if (gameAction.UserId != user)
        {
            gameAction = gameAction with { UserId = user };
        }

        return gameAction;
    }
    
    private static async Task ResponseToClientsAsync(IEnumerable<GameAction> actions, int requestedClient)
    {
        foreach (var action in actions)
        {
            Console.WriteLine($"Server applied action: {action}");
            using var packet = PacketEncoder.EncodeGameAction(action);
            if (action is IOneUserInfo)
                ServerSend.SendTCPData(action.UserId == -1 ? requestedClient : action.UserId, packet);
            else
                ServerSend.SendTCPDataToAll(packet);
            await Task.Delay(50);
        }
    }
}