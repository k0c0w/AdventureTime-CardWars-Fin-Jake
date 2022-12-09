namespace GameServer;

public class ServerSend
{
    public static void SendTCPData(int toClient, Packet packet)
    {
        packet.WriteLength();
        Server.Clients[toClient].Tcp.SendData(packet);
    }
    
    public static void SendTCPDataToAll(Packet _packet)
    {
        _packet.WriteLength();
        for(var i = 1; i <= Server.MaxPlayers; i++)
        {
            Server.Clients[i].Tcp.SendData(_packet);
        }
    }
    public static void SendTCPDataToAll(int exceptClient, Packet packet)
    {
        //writes packetLength;
        packet.WriteLength();
        for (var i = 1; i <= Server.MaxPlayers; i++)
        {
            if(i != exceptClient) Server.Clients[i].Tcp.SendData(packet);
        }
    }

    public static void Welcome(int toClient, string message)
    {
        using var packet = new Packet((int)ServerPackets.Welcome);
        packet.Write(message);
        packet.Write(toClient);
        SendTCPData(toClient, packet);
    }

    public static void SpawnPlayer(int toClient, Player player)
    {
        using var packet = new Packet((int)ServerPackets.SpawnPlayer);
        packet.Write(player.Id);
        packet.Write(player.Username);

        SendTCPData(toClient,packet);
    }
}