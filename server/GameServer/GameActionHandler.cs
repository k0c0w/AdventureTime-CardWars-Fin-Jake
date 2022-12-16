using GameKernel;

namespace GameServer;

public class GameActionHandler
{
    public static void ApplyToGame(int fromClient, Packet packet)
    {
        try
        {
            ResponseToClients(Server.CurrentGame.ApplyGameActions(GetGameAction(fromClient, packet)), fromClient);
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
    
    private static void ResponseToClients(IEnumerable<GameAction> actions, int requestedClient)
    {
        foreach (var action in actions)
        {
            var packet = PacketEncoder.EncodeGameAction(action);
            if (actions is IOneUserInfo)
                ServerSend.SendTCPData(requestedClient, packet);
            else
                ServerSend.SendTCPDataToAll(packet);
        }
    }
}