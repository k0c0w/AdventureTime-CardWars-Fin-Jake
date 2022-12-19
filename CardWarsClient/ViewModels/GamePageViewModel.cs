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

namespace CardWarsClient.ViewModels
{
    public partial class GamePageViewModel : ObservableObject
    {
        public ObservableCollection<CardModel> hand { get; set; } = new ObservableCollection<CardModel>();
        public ObservableCollection<LandModel> lands { get; set; } = new ObservableCollection<LandModel>();

        private CardModel _dragged;

        [ObservableProperty]
        public string showingImage = null;

        public GamePageViewModel()
        {
            hand.Add(new CardModel { imagePath = "archer_dan.png" });
            hand.Add(new CardModel { imagePath = "cobs_legion.png" });
            hand.Add(new CardModel { imagePath = "archer_dan.png" });
            hand.Add(new CardModel { imagePath = "shy_bard.png" });

            lands.Add(new LandModel { Id = 0, imagePath = "blue_land.png" });
            lands.Add(new LandModel { Id = 1, imagePath = "blue_land.png" });
            lands.Add(new LandModel { Id = 2, imagePath = "blue_land.png" });
            lands.Add(new LandModel { Id = 3, imagePath = "reversed_land.png" });

            ShowingImage = hand[0].imagePath;
        }

        [RelayCommand]
        public void StartDrag(CardModel card)
        {
            _dragged = card;
        }

        [RelayCommand]
        public void DropCard(int slot)
        {

            //todo: wait verify from server

            if(_dragged != null)
            {
                hand.Remove(_dragged);
                lands[slot] = new LandModel { Id = lands[slot].Id, imagePath = lands[slot].imagePath, bindedCard = _dragged }; // костыль
                _dragged = null;
            }
        }

        [RelayCommand]
        public void ChangePreview(string path)
        {
            ShowingImage = path;
        }
    }
}
