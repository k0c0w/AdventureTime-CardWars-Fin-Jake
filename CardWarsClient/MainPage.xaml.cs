using Microsoft.Maui.Layouts;
using Shared.Packets;
using Windows.Networking.NetworkOperators;

namespace CardWarsClient;

public partial class MainPage : ContentPage
{
    bool isReady = false;

    public MainPage()
	{
        InitializeComponent();
    }

    private async void ReadyClicked(object sender, EventArgs e)
    {
        //await Shell.Current.GoToAsync("GamePage");
        var readyPacket = new Packet((int)PacketId.ClientPacket);
        isReady = !isReady;
        ClientSend.ReadyChange(isReady);
        
        ReadyBtn.Text = isReady ? "Не готов" : "Готов";
    }

    private void ConnectClicked(object sender, EventArgs e)
    {

        Client.Instance.Start();
        try
        {
            Client.Instance.ConnectToServer();
        }
        catch
        {
            Application.Current.Quit();
        }

        ReadyBtn.IsVisible = true;
        nameText.Text = "Ждём игроков...";
        entry.IsVisible = false;
        ConnectBtn.IsVisible = false;
    }

    private void OnEntryTextChanged(object sender, TextChangedEventArgs e)
    {
        Client.Instance.Username = e.NewTextValue;
    }
}

