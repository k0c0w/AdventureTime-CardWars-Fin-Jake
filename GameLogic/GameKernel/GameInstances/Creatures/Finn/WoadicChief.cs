using Shared.PossibleCards;

namespace GameObjects.Creatures;

public class WoadicChief : Creature
{
    public WoadicChief(int line, Player owner) : base(line, owner, LandType.blue_plains, AllCards.woadic_chief, 2, 10, 2)
    {
    }

    public override void ExecuteSkill()
    {
        //todo: +2 к атаке, за каждое сыгранное заклинание в этот ход
    }
}