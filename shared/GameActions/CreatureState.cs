namespace Shared.GameActions;

public record CreatureState : GameAction, IBothUserInfo
{
    public int Owner { get; }

    public int Line { get; }

    public int HPBefore { get; init; }
    
    public int HPAfter { get; init; }
    
    public bool IsDead { get; init; }

    public bool IsFlooped { get; init; }

    public CreatureState(int owner, int line)
    {
        UserId = -1;
        Owner = owner;
        Line = line;
    }
}