using Shared.PossibleCards;

namespace GameObjects.Creatures;

public class Pig : Creature, IFloopable
{
    public Pig(int line, Player owner) : base(line, owner, LandType.any, AllCards.pig, 1, 4, 1)
    {
    }

    public override void ExecuteSkill() { }
    
    public bool CanBeFlooped() => !IsFlooped;

    public void Floop()
    {
        IsFlooped = true;
        var opponentLandLine = Owner.Opponent.Lands[Line];
        if (!opponentLandLine.IsTurnedOver)
            opponentLandLine.TurnOver();
        //todo: Owner.CurrentGame.RegisterAction(new UserFloopCard());
    }
}