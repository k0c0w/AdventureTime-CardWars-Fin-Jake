using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm;
using System.Collections.ObjectModel;
using CardWarsClient.Models;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CardWarsClient.Models
{
    public class LandModel
    {
        public int Id { get; set; }
        public string imagePath { get; set; }

        public CardModel bindedCard { get; set; } //= new CardModel { hasDamage = false, takenDamage = 0 };
    }
}
