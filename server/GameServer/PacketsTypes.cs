namespace GameServer;

/// <summary>Sent from server to client.</summary>
public enum ServerPacket
{
    Welcome,
    GoodBye,
}

/// <summary>Sent from client to server.</summary>
public enum ClientPacket
{
    WelcomeReceived = 0,
    ReadyForGame
}

public enum GameActionPacket
{
    GameStart
}

public enum PacketId
{
    ServerPacket = 0,
    ClientPacket,
    GameActionPacket
}