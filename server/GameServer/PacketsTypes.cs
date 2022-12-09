namespace GameServer;

/// <summary>Sent from server to client.</summary>
public enum ServerPackets
{
    Welcome = 1,
    SpawnPlayer,
}

/// <summary>Sent from client to server.</summary>
public enum ClientPackets
{
    WelcomeReceived = 1,
    PlayerMovement
}