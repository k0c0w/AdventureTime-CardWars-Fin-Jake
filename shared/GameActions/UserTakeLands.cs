using Shared.PossibleCards;

namespace Shared.GameActions;

public record UserTakeLands : GameAction, IBothUserInfo
{
    public LandType[] Lands { get; }
    
    public UserTakeLands(int user, LandType[] lands)
    {
        if (lands.Length != 4 || lands.Contains(LandType.any))
            throw new ArgumentException("Invalid Lands");
        UserId = user;
        Lands = lands;
    }
}