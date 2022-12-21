using CardWarsClient.ViewModels;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Windows.UI.Notifications;

namespace CardWarsClient;

public partial class GamePage : ContentPage
{
	public GamePage()
	{
		InitializeComponent();
		this.BindingContext = GamePageViewModel.Instance;
	}
}