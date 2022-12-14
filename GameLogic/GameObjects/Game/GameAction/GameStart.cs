namespace GameKernel;

public record GameStart : GameAction
{
    public GameStart()
    {
        UserId = -1;
    }
}