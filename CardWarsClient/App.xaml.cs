namespace CardWarsClient;

public partial class App : Application
{
    public static Client r = new Client();
    public App()
	{
        Client.Instance = r;
        r.Start();
        try
        {
            r.ConnectToServer();
        }
        catch
        {
            Application.Current.Quit();
        }

        InitializeComponent();

		MainPage = new AppShell();
	}
}
