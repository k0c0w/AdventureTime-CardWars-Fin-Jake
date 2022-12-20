using CommunityToolkit.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardWarsClient.Models;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using static System.Collections.Specialized.BitVector32;
using Shared.PossibleCards;
using System.ComponentModel;
using Shared.Decks;

namespace CardWarsClient.ViewModels
{
    public partial class GamePageViewModel : ObservableObject
    {
        public static GamePageViewModel Instance { get; set; }
        public ObservableCollection<CardModel> hand { get; set; } = new ObservableCollection<CardModel>();
        public ObservableCollection<LandModel> lands { get; set; } = new ObservableCollection<LandModel>();

        public DeckTypes Deck;

        private CardModel _dragged;
        private int _actionsCount;

        [ObservableProperty]
        public string showingImage = null;

        [ObservableProperty]
        public bool isCurrentPlayerTurn = true;

        [ObservableProperty]
        public string availableActionsPrompt;

        public GamePageViewModel()
        {
            Instance = this;
            hand.Add(new CardModel { Name = AllCards.corn_ronin, imagePath = "corn_ronin.png", Cost = 1, takenDamage = 2, hasDamage = true});
            hand.Add(new CardModel { Name = AllCards.corn_ronin, imagePath = "corn_ronin.png", Cost = 1, takenDamage = 3, hasDamage = true});
            hand.Add(new CardModel { Name = AllCards.spirit_solder, imagePath = "spirit_solder.png", Cost = 1 });
            hand.Add(new CardModel { Name = AllCards.spirit_solder, imagePath = "spirit_solder.png", Cost = 1 });

            lands.Add(new LandModel { Id = 0, imagePath = "blue_land.png" });
            lands.Add(new LandModel { Id = 1, imagePath = "blue_land.png" });
            lands.Add(new LandModel { Id = 2, imagePath = "blue_land.png" });
            lands.Add(new LandModel { Id = 3, imagePath = "reversed_land.png" });

            ShowingImage = hand[0].imagePath;
            AvailableActionsPrompt = "Доступные действия: 2";
            _actionsCount = 2;
        }

        [RelayCommand]
        public void StartDrag(CardModel card)
        {
            _dragged = card;
        }

        [RelayCommand]
        public void DropCard(int slot)
        {
            if(_dragged != null && _actionsCount >= _dragged.Cost)
            {
                ClientSend.PutCard(_dragged.Name, slot, hand.IndexOf(_dragged));

                //todo: wait verify from server

                hand.Remove(_dragged);
                _actionsCount -= _dragged.Cost;
                AvailableActionsPrompt = $"Доступные действия: {_actionsCount}";
                //var _fixedCard = new CardModel { Name=_dragged.Name, Cost= _dragged.Cost, hasDamage = _dragged.hasDamage, imagePath = _dragged.imagePath, isFlupped = _dragged.isFlupped, takenDamage = _dragged.takenDamage };
                lands[slot] = new LandModel { Id = lands[slot].Id, imagePath = lands[slot].imagePath, bindedCard = _dragged }; // костыль
                _dragged = null;
            } 
            else
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await Shell.Current.DisplayAlert("Ошибка", "Нельзя положить эту карту!", "ОK");
                });
            }
        }

        [RelayCommand]
        public void ChangePreview(string path)
        {
            ShowingImage = path;
        }
        
        [RelayCommand]
        public void ChangeTurn()
        {
            IsCurrentPlayerTurn = !IsCurrentPlayerTurn;

            ClientSend.EndTurn();
            //todo: wait for opponent turn and lock hand...
        }
    }
}
