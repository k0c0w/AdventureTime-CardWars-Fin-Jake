namespace GameServer;

public record class Player
{
    public int Id { get; init; }
    
    public string Username { get; init; }
    
    public bool IsReady { get; set; }
    public Player() {}

    public Player(int id, string username)
    {
        Id = id;
        Username = username;
    }
}