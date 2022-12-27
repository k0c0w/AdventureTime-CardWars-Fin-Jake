namespace Shared.GameActions;

public record GameStart : GameAction, IBothUserInfo
{
    public (int, string) FirstPlayerInfo { get; init; }
    
    public (int, string) SecondPlayerInfo { get; init; }

    public GameStart()
    {
        UserId = -1;
    }
}