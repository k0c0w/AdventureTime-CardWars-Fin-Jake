using GameKernel.Deck;
using GameObjects;
using GameKernel.GameStates;
using GameKernel.temp;

namespace GameKernel;

public class Game
{
    public IGameState GameState { get; internal set; }

    internal readonly Dictionary<int, Player> Players;
    internal (DeckTypes, DeckTypes) AllowedDecks { get; }
    internal DecksAndLandsHolder _holder;

    private Creature?[][] PlayersCreatures { get; set; }
    
    private Building?[][] PlayersBuildings { get; set; }
    
    private List<GameInstance>[] PlayersHand { get; set; }
    
    
    private readonly Queue<GameAction> _gameActions = new Queue<GameAction>();

    //todo: аргументы: ввести игроков (которые соединения)
    public Game(GameSetting gameSetting)
    {
        Players = new Dictionary<int, Player>();
        _holder = LandsFactory.PrepareLandsAndDecks((dynamic)gameSetting);
        AllowedDecks = gameSetting.Decks;
        PlayersCreatures = new Creature?[][2];
        PlayersBuildings = new Building?[][2];
        PlayersHand = new List<GameInstance>[2];
    }
    
    public IEnumerable<GameAction> ApplyGameActions(GameAction action)
    {
        if (!GameState.IsValidAction(action))
            return BadRequest();
        GameState.Execute(action);
        var actions = _gameActions;
        _gameActions.Clear();
        return actions;
    }

    //todo: ассинхронная операция
    internal void RegisterAction(GameAction action) => _gameActions.Enqueue(action);

    internal void RegisterPlayer(Player player)
    {
        //todo: assign deck и сброс
        var id = player.Id;
        Players.Add(id, player);
        PlayersCreatures[id] = player.Creatures;
        PlayersHand[id] = player.Hand;
        PlayersBuildings[id] = player.Buildings; 
    }

    private IEnumerable<GameAction> BadRequest()
    {
        throw new NotImplementedException();
    }
}