using GameKernel;
using Shared.PossibleCards;

namespace GameObjects;

//todo: player : Idisposable => ссылки на gameInstance убрать
public class Player
{
    //todo: исправить deck
    public Deck Deck { get; internal set; }
    
    public int Id { get;}

    public Player Opponent => CurrentGame.Players.Values.First(x => x.Id != Id);
    
    private Game CurrentGame { get; }

    public List<AllCards> Hand { get; }
    
    public Land[] Lands { get; internal set; }
    
    public Creature?[] Creatures { get; }
    
    public Building?[] Buildings { get; }

    //todo: validate args
    public Player(int userId, Game game)
    {
        //todo: здесь должно быть не GameInstance, а enum карточки
        Hand = new List<AllCards>();
        Creatures = new Creature[4];
        Buildings = new Building[4];
        CurrentGame = game;
        HP = 25;
    }

    public void TakeCard(AllCards card)
    {
        //todo: добавить deck и из нее грабать карту и уведомлять игру здесь 
        Hand.Add(card);
    }

    //todo: метод HasCard чтобы валидировать наличие карты на руке
    
    public void MoveCreatureToLand(int creatureIndex, int landIndex)
    {
        var card = Hand[creatureIndex];
        Hand.RemoveAt(creatureIndex);
        Creatures[landIndex] = CreatureFactory.Summon(this, card, landIndex);
    }

    //повторяющиеся методы - вынести в интерфейс?
    
    public int HP { get; private set; }

    public void TakeDamage(int damage)
    {
        //todo: уведомить игру
        //if(damage < 0) throw new ArgumentException();
        HP -= damage;
    }

    public int ControlledLands(LandType land) => Lands.Count(x => !x.IsTurnedOver && x.LandType == land);

    public bool IsDead => HP <= 0;
}