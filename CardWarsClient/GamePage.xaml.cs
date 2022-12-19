using CardWarsClient.ViewModels;
using Windows.UI.Notifications;

namespace CardWarsClient;

public partial class GamePage : ContentPage
{
	public GamePage()
	{
		InitializeComponent();
		this.BindingContext = new GamePageViewModel();
	}
}