using CardWarsClient.ClientLogic.Cards;
using CommunityToolkit.Mvvm.ComponentModel;
using Shared.Decks;
using Shared.PossibleCards;
using System.Collections.ObjectModel;

namespace CardWarsClient.Models
{
    [ObservableObject]
    public partial class PlayerModel
    {
        public ObservableCollection<LandModel> Lands { get; set; }

        public ObservableCollection<CardModel> Hand { get; }

        public PlayerModel(int landsIdShift = 0, bool handInit = false) 
        {
            Lands = new ObservableCollection<LandModel>();
            var lands = Lands;
            for (var i = 0; i < 4; i++)
                lands.Add(new LandModel { Id = i + landsIdShift });
            if(handInit)
                Hand = new ObservableCollection<CardModel>();
        }


        private DeckTypes _deck;

        [ObservableProperty]
        private int hp = 25;

        [ObservableProperty]
        public string sourcePath;

        public int Id { get; set; }

        public DeckTypes Deck
        {
            get => _deck;
            set
            {
                SetProperty(ref _deck, value);
                SourcePath = string.Format("{0}avatar.png", _deck.ToString().Replace("Deck", "").ToLower());
            }
        }

        public void TakeInitialHand(AllCards[] cards)
        {
            foreach (var card in cards)
                Hand.Add(new CardModel { Name = card });
        }

        public void TakeLands(LandType[] lands)
        {
            for (var i = 0; i < 4; i++)
            {
                var land = Lands[i];
                land.Land = lands[i];
                land.TurnOut();
            }
        }
    }
}