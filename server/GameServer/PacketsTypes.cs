namespace GameServer;

/// <summary>Sent from server to client.</summary>
public enum ServerPacket
{
    Welcome = 1,
    SpawnPlayer,
}

/// <summary>Sent from client to server.</summary>
public enum ClientPacket
{
    WelcomeReceived = 1,
    PlayerMovement
}

public enum PacketId
{
    ServerPacket = 0,
    ClientPacket
}