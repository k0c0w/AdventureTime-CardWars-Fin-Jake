namespace Shared.Packets;

public enum ServerPacket
{
    Welcome,
    GoodBye,
    ServerError
}

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
    UserTakeDeck,
    UserDecisionStart,
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