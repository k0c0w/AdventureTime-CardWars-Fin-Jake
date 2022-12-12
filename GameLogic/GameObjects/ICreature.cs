namespace GameObjects;

public interface ICreature
{
    bool IsFlipped();

    bool IsDead();

    void Flip();

    void Attack(ICreature enemy);

    void TakeDamage(int damage);

    void RegisterBuff(int buff);
}