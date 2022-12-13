using GameObjects.Buildings;
using GameObjects.Creatures;

namespace GameObjects;

//todo: player : Idisposable => ссылки на gameInstance убрать
public class Player : IDisposable
{
    //todo: поменять лиюо на game либо добавить инит
    public Player Opponent;
    
    public int Id { get; }
    
    public List<GameInstance> Hand { get; }
    
    public Land[] Lands { get; }
    
    public Creature?[] Creatures { get; }
    
    public Building?[] Buildings { get; }

    //todo: validate args
    public Player(int id, Land[] lands)
    {
        //todo: здесь должно быть не GameInstance, а enum карточки
        Hand = new List<GameInstance>();
        Creatures = new Creature[4];
        Buildings = new Building[4];
        Lands = lands;
        HP = 25;
        Id = id;
    }

    public void TakeCard(GameInstance card)
    {
        Hand.Add(card);
    }

    //todo: метод HasCard чтобы валидировать наличие карты на руке
    
    public void MoveCreatureToLand(int creatureIndex, int landIndex)
    {
        var card = Hand[creatureIndex];
        Hand.RemoveAt(creatureIndex);
        //todo: бросать кастомную ошибку, чтобы в логике потом catch через multipledispatch и возвращать BadRequest
        Creatures[landIndex] = (Creature)card;
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
    public void Dispose()
    {
        Opponent = null;
    }

    ~Player() => Dispose();
}