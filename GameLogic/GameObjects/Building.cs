namespace GameObjects;

public abstract class Building : GameInstance
{
    public int Line { get; set; }
    protected Building(int line, Player owner, LandType land, int cost) : base(owner, land, cost)
    {
        Line = line;
    }

    public void Destroy()
    {
        Owner.Buildings[Line] = null;
        //todo: уведомить игру о действии
    }
}