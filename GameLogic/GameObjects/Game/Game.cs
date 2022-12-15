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

    private Dictionary<int, Creature?[]> PlayersCreatures { get; }
    
    private Dictionary<int, Building?[]> PlayersBuildings { get; }
    
    private Dictionary<int, List<GameInstance>> PlayersHand { get; }
    
    
    private readonly Queue<GameAction> _gameActions = new Queue<GameAction>();

    public Game(GameSetting gameSetting, int player1Id, int player2Id)
    {
        //todo: ограничить id игроков - только 1 и 2
        if (player1Id == -1 || player2Id == -1)
            throw new ArgumentException("Id -1 is reserved by game");
        var player1 = new Player(player1Id, this);
        var player2 = new Player(player2Id, this);
        Players = new Dictionary<int, Player>() { { player1Id, player1 }, { player2Id, player2 } };
        _holder = LandsFactory.PrepareLandsAndDecks((dynamic)gameSetting);
        AllowedDecks = gameSetting.Decks;
        PlayersCreatures = new Dictionary<int, Creature?[]>()
            { { player1Id, player1.Creatures }, { player2Id, player2.Creatures } };
        PlayersBuildings = new Dictionary<int, Building?[]>()
            { { player1Id, player1.Buildings }, { player2Id, player2.Buildings } };
        PlayersHand =  new Dictionary<int, List<GameInstance>>()
            { { player1Id, player1.Hand }, { player2Id, player2.Hand } };
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