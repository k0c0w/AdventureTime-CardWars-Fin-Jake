namespace GameKernel;

public record GameStart : GameAction, IBothUserInfo
{
    public GameStart()
    {
        UserId = -1;
    }
}