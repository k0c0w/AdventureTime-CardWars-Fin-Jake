namespace CardWarsClient;

public partial class App : Application
{
    public static Client r = new Client();
    public App()
	{
        Client.Instance = r;
        InitializeComponent();
        MainPage = new AppShell();
	}
}
