namespace GameObjects;

//todo: player : Idisposable => ссылки на gameInstance убрать
public class Player
{
    public int Id { get; }
    
    public List<GameInstance> Hand { get; }
    
    public Land[] Lands { get; }
    
    public Creature[] Creatures { get; }
    
    public Building[] Buildings { get; }

    public Player(int id, Land[] lands)
    {
        //todo: validate args
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
        //if(damage < 0) throw new ArgumentException();
        HP -= damage;
    }

    public bool IsDead => HP <= 0;
}