namespace GameServer;

/// <summary>Sent from server to client.</summary>
public enum ServerPacket
{
    Welcome,
    GoodBye,
    ServerError
}

/// <summary>Sent from client to server.</summary>
public enum ClientPacket
{
    WelcomeReceived = 0,
    ReadyForGame
}

public enum GameActionPacket
{
    GameStart,
    Winner,
    BadRequest,
    PossibleDecks,
    UserChoseDeck,
    UserDecisionEnd,
    UserPutCard,
    GameEnd,
}

public enum PacketId
{
    ServerPacket = 0,
    ClientPacket,
    GameActionPacket
}