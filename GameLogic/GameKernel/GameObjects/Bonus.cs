namespace GameObjects;

public record Bonus
{
    public int DefenceBonus { get; init; } = 0;

    public int AttackBonus { get; init; } = 0;
    
    public Bonus() {}

    public Bonus(int defenceBonus, int attackBonus)
    {
        DefenceBonus = defenceBonus;
        AttackBonus = attackBonus;
    }
}