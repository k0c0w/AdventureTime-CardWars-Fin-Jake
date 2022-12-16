namespace GameObjects.Creatures;

public class GhostOfFields : Creature
{
    public GhostOfFields(int line, Player owner) : base(line, owner, LandType.CornFields, 1, 10, 1)
    {
    }
    
    //todo: подписаться на событие игры о начале хода игрока owner

    public override void ExecuteSkill()
    {
        throw new NotImplementedException();
    }
}