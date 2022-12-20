

namespace CardWarsClient.Models
{
    public class LandModel
    {
        public int Id { get; set; }

        public string imagePath { get; set; }

        public CardModel bindedCard { get; set; } //= new CardModel { hasDamage = false, takenDamage = 0 };
    }
}
