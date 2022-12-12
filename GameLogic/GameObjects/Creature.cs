namespace GameObjects;

public class Creature : GameInstance, ICreature
{
    public int HP { get; private set; }
    
    public int Damage { get; private set; }

    private bool _isFlipped;

    private int _buffDamage = 0;


    public Creature(IPlayer owner, LandType land, int cost, int healthPoints, int damage) : base(owner, land, cost)
    {
        if (healthPoints <= 0 || damage <= 0)
            throw new ArgumentException($"The {nameof(healthPoints)} or {nameof(damage)} must be > 0.");
        HP = healthPoints;
        Damage = damage;
    }
    
    public bool IsFlipped() => _isFlipped;

    public bool IsDead() => HP <= 0;
    
    public void Flip() => _isFlipped = !_isFlipped;

    public void RegisterBuff(int buff) => _buffDamage += buff;

    public void Attack(ICreature enemy)
    {
        enemy.TakeDamage(Damage + _buffDamage);
        _buffDamage = 0;
    }

    public void TakeDamage(int damage) => HP -= damage;
}