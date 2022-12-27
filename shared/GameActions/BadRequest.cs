namespace Shared.GameActions;

public record BadRequest : GameAction, IOneUserInfo
{
    public BadRequest() => UserId = -1;
}