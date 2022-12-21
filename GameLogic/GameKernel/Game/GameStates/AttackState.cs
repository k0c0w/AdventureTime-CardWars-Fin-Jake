using GameObjects;
using Shared.GameActions;

namespace GameKernel.GameStates;

public class AttackState : IGameState
{
    private readonly int _attacking;
    
    public AttackState(int attackingPlayer, Game game)
    {
        CurrentGame = game;
        _attacking = attackingPlayer;
    }

    public Game CurrentGame { get; }
    public bool IsValidAction(GameAction action) => false;

    public void Execute(GameAction action)
    {
        var creaturesOnLands = CurrentGame.PlayersCreatures[_attacking].Where(x => x is { IsAttacking: true });
        RegisterBonuses(creaturesOnLands);
        RegisterBonuses(CurrentGame.PlayersCreatures[CurrentGame.OpponentIdTo(_attacking)]
            .Where(x => x is { IsAttacking: true })!);
        DamageEnemies(creaturesOnLands);
        ChangeState();
    }

    public void ChangeState()
    {
        var opponent = CurrentGame.OpponentIdTo(_attacking);
        if (CurrentGame.Players[_attacking].IsDead || CurrentGame.Players[opponent].IsDead)
            CurrentGame.GameState = new WinnerState(CurrentGame);
        else
        {
            CurrentGame.GameState = new TakeCardsState(opponent, CurrentGame);
            CurrentGame.GameState.Execute(new GameAction() {UserId = opponent});
        }
    }

    private void RegisterBonuses(IEnumerable<Creature> creatures)
    {
        foreach (var creature in creatures)
        {
            creature.Reset();
            creature.ExecuteSkill();
        }
    }

    private void DamageEnemies(IEnumerable<Creature> attackingCreatures)
    {
        var opId = CurrentGame.OpponentIdTo(_attacking);
        var opponent = CurrentGame.Players[opId];
        var opponentCreatures = CurrentGame.PlayersCreatures[opId];
        foreach (var creature in attackingCreatures)
        {
            if (opponentCreatures[creature.Line] == null)
            {
                var damage = creature.Damage;
                //todo: может лучше подсчитывать полный урон и только потом отправлять hp
                opponent.TakeDamage(damage);
                CurrentGame.RegisterAction(new UserTakeDamage(opId, damage));
            }
            else
                DamageEnemyCreature(creature, opponentCreatures[creature.Line]);
        }
    }

    private void DamageEnemyCreature(Creature attacking, Creature enemy)
    {
        var enemyHpBefore = enemy.HP;
        var attackingHpBefore = attacking.HP;
        attacking.Attack(enemy);
        enemy.Attack(attacking);
        RegisterCreatureState(enemyHpBefore, enemy);
        RegisterCreatureState(attackingHpBefore, attacking);
        if(attacking.IsDead)
            attacking.Destroy();
        if(enemy.IsDead)
            enemy.Destroy();
    }
    

    private void RegisterCreatureState(int hpBefore, Creature creature)
    {
        CurrentGame.RegisterAction(new CreatureState(creature.Owner.Id, creature.Line)
        {
            HPBefore = hpBefore,
            HPAfter = creature.HP,
            IsDead = creature.IsDead
        });
    }
}