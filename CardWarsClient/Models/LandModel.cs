using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Shared.PossibleCards;

namespace CardWarsClient.Models
{
    [ObservableObject]
    public partial class LandModel
    {
        public bool IsTurnedOut => ImagePath == reversed;

        public LandType Land { get; set; }

        private static string reversed = "reversed_land.png";

        public int Id { get; set; }

        [ObservableProperty]
        public string imagePath = reversed;

        [ObservableProperty]
        public CardModel bindedCard;

        public void TurnOut() => ImagePath = IsTurnedOut ? $"{Land}.png" : reversed;
    }
}
