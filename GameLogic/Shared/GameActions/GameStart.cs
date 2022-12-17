namespace Shared.GameActions;

public record GameStart : GameAction, IBothUserInfo
{
    public GameStart()
    {
        UserId = -1;
    }
}