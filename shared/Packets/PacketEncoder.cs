using Shared.GameActions;
using Shared.Decks;
using Shared.PossibleCards;
using Microsoft.CSharp.RuntimeBinder;

namespace Shared.Packets;

public class PacketEncoder
{
    public static Packet EncodeGameAction(GameAction gameAction)
    {
        try
        {
            var packet = new Packet();
            packet.Write((int)PacketId.GameActionPacket);
            return Encode((dynamic)gameAction, packet);
        }
        catch(RuntimeBinderException)
        {
            Console.WriteLine($"Не удалось закодировать пакет: {gameAction.GetType().Name}");
            throw;
        }
    }

    private static Packet Encode(UserDecisionStart request, Packet packet)
    {
        packet.Write((int)GameActionPacket.UserDecisionStart);
        return packet;
    }
    
    private static Packet Encode(BadRequest request, Packet packet)
    {
        packet.Write((int)GameActionPacket.BadRequest);
        return packet;
    }
    
    private static Packet Encode(GameStart request, Packet packet)
    {
        packet.Write((int)GameActionPacket.GameStart);
        return packet;
    }

    private static Packet Encode(PossibleDecks request, Packet packet)
    {
        packet.Write((int)GameActionPacket.PossibleDecks);
        packet.Write((int)request.First);
        packet.Write((int)request.Second);
        return packet;
    }

    private static Packet Encode(UserChoseDeck request, Packet packet)
    {
        packet.Write((int)GameActionPacket.UserChoseDeck);
        packet.Write(request.UserId);
        packet.Write((int)request.DeckType);
        return packet;
    }

    private static Packet Encode(UserPutCard request, Packet packet)
    {
        packet.Write((int)GameActionPacket.UserPutCard);
        packet.Write(request.UserId);
        packet.Write(request.Line);
        packet.Write((int)request.Card);
        packet.Write((int)request.IndexInHand!);
        return packet;
    }

    private static Packet Encode(UserDecisionEnd request, Packet packet)
    {
        packet.Write((int)GameActionPacket.UserDecisionEnd);
        packet.Write(request.UserId);
        return packet;
    }

    private static Packet Encode(UserTakeDeck request, Packet packet)
    {
        packet.Write((int)GameActionPacket.UserTakeDeck);
        var cards = request.CardsFromDeck;
        var lands = request.Lands;
        for(var i = 0; i < 5; i++)
            packet.Write((int)cards[i]);
        for(var i = 0; i < 4; i++)
            packet.Write((int)lands[i]);
        return packet;
    }

    public static GameAction DecodeGameAction(Packet packet)
    {
        /*var packetType = (PacketId)packet.ReadInt();
        if (packetType != PacketId.GameActionPacket)
            throw new InvalidOperationException($"Incorrect packet type {packetType}.");*/
        var action = (GameActionPacket)packet.ReadInt();
        return action switch
        {
            GameActionPacket.BadRequest => BadRequest,
            GameActionPacket.GameStart => GameStart,
            GameActionPacket.PossibleDecks => DecodePossibleDecks(packet),
            GameActionPacket.UserChoseDeck => DecodeUserChoseDeck(packet),
            GameActionPacket.UserPutCard => DecodeUserPutCard(packet),
            GameActionPacket.UserDecisionStart => DecodeUserDecisionStart(packet),
            GameActionPacket.UserDecisionEnd => DecodeUserDecisionEnd(packet),
            GameActionPacket.UserTakeDeck => DecodeUserTakeDeck(packet),
            GameActionPacket.GameEnd => throw new NotImplementedException(),
            _ => throw new InvalidOperationException()
        };
    }

    private static PossibleDecks DecodePossibleDecks(Packet packet)
    {
        var first = (DeckTypes)packet.ReadInt();
        var second = (DeckTypes)packet.ReadInt();
        return new PossibleDecks { First = first, Second = second, UserId = -1};
    }

    private static UserChoseDeck DecodeUserChoseDeck(Packet packet)
    {
        var user = packet.ReadInt();
        var deck = (DeckTypes)packet.ReadInt();
        return new UserChoseDeck(user, deck);
    }
    
    private static BadRequest BadRequest { get; } = new BadRequest();
    private static GameStart GameStart { get; } = new GameStart();
    
    private static UserPutCard DecodeUserPutCard(Packet packet)
    {
        var client = packet.ReadInt();
        var line = packet.ReadInt();
        var card = (AllCards)packet.ReadInt();
        var index = packet.ReadInt();
        return new UserPutCard(client, line, card, index);
    }

    private static UserDecisionStart DecodeUserDecisionStart(Packet packet) =>
        new UserDecisionStart { UserId = packet.ReadInt() };

    private static UserDecisionEnd DecodeUserDecisionEnd(Packet packet) =>
        new UserDecisionEnd { UserId = packet.ReadInt() };

    private static UserTakeDeck DecodeUserTakeDeck(Packet packet)
    {
        var userId = packet.ReadInt();
        var hand = new AllCards[5];
        var lands = new LandType[4];
        for (var i = 0; i < 5; i++)
            hand[i] = (AllCards)packet.ReadInt();
        for (var i = 0; i < 4; i++)
            lands[i] = (LandType)packet.ReadInt();

        return new UserTakeDeck(userId, hand, lands);
    }
}