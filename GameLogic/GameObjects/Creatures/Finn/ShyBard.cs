namespace GameObjects.Creatures;

public class ShyBard : Creature, IFlupable
{
    public ShyBard(int line, Player owner) : base(line, owner, LandType.BluePlains, 2, 5, 1)
    { }

    public override void ExecuteSkill()
    {
        if(!IsFlupped()) return;
        var count = Owner.Creatures.Count(x => x != null);
        //todo: игрок тянет карту здесь в колве count
    }

    public bool CanBeFlupped()
    {
        throw new NotImplementedException();
    }

    public void Flup()
    {
        throw new NotImplementedException();
    }

    public bool IsFlupped()
    {
        throw new NotImplementedException();
    }
}