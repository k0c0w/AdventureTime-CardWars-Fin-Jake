using CommunityToolkit.Mvvm.ComponentModel;
using Shared.Decks;

namespace CardWarsClient.Models
{
    [ObservableObject]
    public partial class PlayerModel
    {
        private string _source;

        private DeckTypes _deck;

        private int _hp = 25;

        public int Id { get; set; }

        public int HP 
        { 
            get => _hp; 
            set 
            { 
                
                SetProperty(ref _hp, value); 
            } 
        }

        public DeckTypes Deck
        {
            get => _deck; 
            set
            {
                SetProperty(ref _deck, value);
            }
        }

        [ObservableProperty]
        public string sourcePath;

        //public bool IsVisible => !string.IsNullOrEmpty(_source);
    }
}
