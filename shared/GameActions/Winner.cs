namespace Shared.GameActions;

public record Winner : GameAction, IBothUserInfo
{
    public Winner(int winner) => UserId = winner;
}