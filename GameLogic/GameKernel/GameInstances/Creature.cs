using Shared.PossibleCards;

namespace GameObjects;

public abstract class Creature : GameObject
{
    public int Line { get; set; }
    
    public bool IsAttacking{ get; protected set; }
    
    public bool IsDead => HP < 0;
    
    public int HP => _cretureHP + _buffDamage;

    public int Damage => _initialDamage + _buffDamage;
    
    private int _buffDamage = 0;
    private int _buffHP = 0;
    private int _cretureHP;
    private readonly int _initialDamage;


    public Creature(int line, Player owner, LandType land, AllCards card, int cost, int healthPoints, int damage) 
        : base(owner, land, card, cost)
    {
        if (healthPoints <= 0 || damage <= 0)
            throw new ArgumentException($"The {nameof(healthPoints)} or {nameof(damage)} must be > 0.");
        _initialDamage = damage;
        _cretureHP = healthPoints;
        //todo: валидировать что действительно там есть создание
        Line = line;
    }

    public void RegisterBonus(Bonus buff)
    {
        _buffHP += buff.DefenceBonus;
        _buffDamage += buff.AttackBonus;
    }

    public void Attack(Creature enemy) 
    {
        enemy.TakeDamage(Damage);
    }

    public void TakeDamage(int damage) => _cretureHP -= damage;

    public void Destroy()
    {
        //todo: special abillities
        Owner.Discard.Push(Card);
        Owner.Creatures[Line] = null!;
    }
    

    protected Creature? ReplaceCreature()
    {
        var old = Owner.Creatures[Line];
        Owner.Creatures[Line] = this;
        return old;
    }

    public void Reset()
    {
        IsAttacking = true;
        _buffDamage = 0;
        _buffHP = 0;
    }
}