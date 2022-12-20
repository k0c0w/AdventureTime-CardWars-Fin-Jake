using CommunityToolkit.Mvvm.ComponentModel;
using Shared.Decks;

namespace CardWarsClient.Models
{
    [ObservableObject]
    public partial class PlayerModel
    {
        private string _source;

        private DeckTypes _deck;

        [ObservableProperty]
        private int hp = 25;

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

        [ObservableProperty]
        public string sourcePath;

    }
}