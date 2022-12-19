namespace Shared.GameActions;

public record UserTakeDamage : GameAction
{
    public int Damage { get; }
    
    public UserTakeDamage(int userId, int damage)
    {
        UserId = userId;
        Damage = damage;
    }
}