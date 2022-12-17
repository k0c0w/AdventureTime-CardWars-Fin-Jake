namespace CardWarsClient;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute("GamePage", typeof(GamePage));
	}
}
