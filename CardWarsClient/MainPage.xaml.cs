using Microsoft.Maui.Layouts;
using Shared.Packets;

namespace CardWarsClient;

public partial class MainPage : ContentPage
{
    bool isReady = false;

    public MainPage()
	{
        InitializeComponent();
    }

    private void ReadyClicked(object sender, EventArgs e)
    {
        var readyPacket = new Packet((int)PacketId.ClientPacket);
        isReady = !isReady;

        //todo: ReadyChange(isReady) in ClientSend instead of this...
        ClientSend.ReadyChange(isReady);
        
        ReadyBtn.Text = isReady ? "Unready" : "Ready";
    }

}

