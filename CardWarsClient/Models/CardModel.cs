using Shared.PossibleCards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CardWarsClient.Models
{
    [ObservableObject]
    public partial record class CardModel
    {
        private AllCards _card;
        public AllCards Name { get => _card; set { imagePath = $"{value}.png"; _card = value; } }
        public string imagePath { get; private set; }
        public int Cost { get; set; }
        public bool isFlupped { get; set; }

        public int takenDamage { get; set; }
   
        public bool hasDamage { get; set; } = false;
    }
}
