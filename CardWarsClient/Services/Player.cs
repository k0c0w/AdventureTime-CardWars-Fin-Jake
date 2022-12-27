using CardWarsClient.Models;
using CardWarsClient.ViewModels;
using Shared.PossibleCards;


namespace CardWarsClient.Services
{
    internal static class Player
    {
        public static int Id { get; set; }
        public static string Username { get; set; }
        public static int ActionsLeft { get; set; }
        public static int HP { get; set; }

        public static List<AllCards> Hand { get; } = new List<AllCards>(8);

        public static LandType[] Lands { get; } = new LandType[4];

        public static void ResetActions() => ActionsLeft = 2;

        public static GamePageViewModel model;
    }
}
