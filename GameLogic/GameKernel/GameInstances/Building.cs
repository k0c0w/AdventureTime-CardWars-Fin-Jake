using Shared.PossibleCards;

namespace GameObjects;

public abstract class Building : GameObject
{
    public int Line { get; set; }
    protected Building(int line, Player owner, LandType land, AllCards card, int cost) : base(owner, land, card, cost)
    {
        Line = line;
    }

    public void Destroy()
    {
        Owner.Buildings[Line] = null;
        //todo: уведомить игру о действии
    }
}