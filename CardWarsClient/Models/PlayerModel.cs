using CommunityToolkit.Mvvm.ComponentModel;
using Shared.Decks;

namespace CardWarsClient.Models
{
    public class PlayerModel : ObservableObject
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
                _source = string.Format("{0}avatar.png", value.ToString().Replace("Deck", "").ToLower()); 
                SetProperty(ref _hp, value); 
            } 
        }

        public DeckTypes Deck { get => _deck; set => SetProperty(ref _deck, value); }

        public string SourcePath => _source;

        public bool IsVisible => !string.IsNullOrEmpty(_source);
    }
}
