using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardWarsClient.Models
{
    public class LandModel
    {
        public int Id { get; set; }
        public string imagePath { get; set; }
        public CardModel bindedCard { get; set; }
    }
}
