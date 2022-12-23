using System.Reflection;
using Shared.PossibleCards;

namespace GameObjects.Creatures;

public class CoolDog : Creature
{
    private readonly FieldInfo _isAttackingField =
        typeof(Creature).GetField("_isAttacking", BindingFlags.NonPublic | BindingFlags.Instance)!;
    
    public CoolDog(int line, Player owner) : base(line, owner, LandType.blue_plains, AllCards.cool_dog, 2, 7, 2)
    {
    }

    public override void ExecuteSkill()
    {
        var targets = GetOpponentCreaturesOnNeighborLines();
        foreach (var enemy in targets)
            _isAttackingField.SetValue(enemy, false);
    }

    private IEnumerable<Creature> GetOpponentCreaturesOnNeighborLines()
    {
        var creatures = new List<Creature>(2);
        var opponentCreatures = Owner.Opponent.Creatures;
        if(Line != 0 && opponentCreatures[Line - 1] != null)
            creatures.Add(opponentCreatures[Line - 1]!);
        if(Line != 3 && opponentCreatures[Line + 1] != null)
           creatures.Add(opponentCreatures[Line + 1]!);
        return creatures;
    }
}