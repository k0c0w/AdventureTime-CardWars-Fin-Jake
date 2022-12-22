using Shared.Decks;
using Shared.PossibleCards;
using Shared.GameActions;
using GameObjects;
using GameKernel.GameStates;

namespace GameKernel;

public class Game
{
    public bool IsFinished { get; internal set; }
    
    internal int OpponentIdTo(int player) => Players.Keys.First(x => x != player);
    
    internal IGameState GameState { get; set; }

    internal readonly Dictionary<int, Player> Players;
    internal (DeckTypes, DeckTypes) AllowedDecks { get; }
    
    internal DecksAndLandsHolder _landsAndDecksHolder;

    internal Dictionary<int, Creature?[]> PlayersCreatures { get; }
    
    internal Dictionary<int, Building?[]> PlayersBuildings { get; }
    
    internal Dictionary<int, List<AllCards>> PlayersHand { get; }
    
    internal Dictionary<int, Deck> PlayersDeck { get; }

    private readonly Queue<GameAction> _gameActions = new Queue<GameAction>();
    private int _creatureIndex = 0;

    public Game(GameSetting gameSetting, int player1Id, int player2Id)
    {
        if (player1Id == -1 || player2Id == -1)
            throw new ArgumentException("Id -1 is reserved by game");
        var player1 = new Player(player1Id, this);
        var player2 = new Player(player2Id, this);
        Players = new Dictionary<int, Player>() { { player1Id, player1 }, { player2Id, player2 } };
        _landsAndDecksHolder = LandsFactory.PrepareLandsAndDecks((dynamic)gameSetting);
        AllowedDecks = gameSetting.Decks;
        PlayersCreatures = new Dictionary<int, Creature?[]>()
            { { player1Id, player1.Creatures }, { player2Id, player2.Creatures } };
        PlayersBuildings = new Dictionary<int, Building?[]>()
            { { player1Id, player1.Buildings }, { player2Id, player2.Buildings } };
        PlayersHand =  new Dictionary<int, List<AllCards>>()
            { { player1Id, player1.Hand }, { player2Id, player2.Hand } };
        PlayersDeck = new Dictionary<int, Deck> { { player1Id, player1.Deck }, { player2Id, player2.Deck } };
        GameState = new DeckChooseGameState(this);
    }
    
    public IEnumerable<GameAction> ApplyGameActions(GameAction action)
    {
        if (!GameState.IsValidAction(action))
            return BadRequest();
        GameState.Execute(action);
        var actions = new GameAction[_gameActions.Count];
        _gameActions.CopyTo(actions, 0);
        _gameActions.Clear();
        return actions;
    }

    internal void RegisterAction(GameAction action) => _gameActions.Enqueue(action);

    internal bool TryPlayCreature(Player player, AllCards cardType, int line)
    {
        try
        {
            player.MoveCreatureToLand(cardType, line);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    internal static GameAction BadRequestAction { get; } = new BadRequest();
    private static IEnumerable<GameAction> BadRequest() => new GameAction[1] { Game.BadRequestAction };
}
