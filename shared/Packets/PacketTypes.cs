namespace Shared.Packets;

public enum ServerPacket
{
    Welcome,
    GoodBye,
    ServerError,
    AlreadyConnected
}

public enum ClientPacket
{
    WelcomeReceived = 0,
    ReadyForGame
}

public enum GameActionPacket
{
    GameStart,
    BadRequest,
    PossibleDecks,
    UserChoseDeck,
    UserTakeLands,
    UserTakeCards,
    UserDecisionStart,
    UserDecisionEnd,
    UserPutCard,
    UserTakeDamage,
    CreatureState,
    Winner,
}

public enum PacketId
{
    ServerPacket = 0,
    ClientPacket,
    GameActionPacket
}