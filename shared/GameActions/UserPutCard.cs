using Shared.PossibleCards;

namespace Shared.GameActions;

public record UserPutCard : CardAction, IBothUserInfo
{
    public UserPutCard(int userId, int line, AllCards card) : base(userId, line, card){}
    
    public int EnergyLeft { get; init; } = 0;
} 