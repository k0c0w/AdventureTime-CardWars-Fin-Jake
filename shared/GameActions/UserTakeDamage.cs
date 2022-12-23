namespace Shared.GameActions;

public record UserTakeDamage : GameAction, IBothUserInfo
{
    public int Damage { get; }
    
    public UserTakeDamage(int userId, int damage)
    {
        UserId = userId;
        Damage = damage;
    }
}