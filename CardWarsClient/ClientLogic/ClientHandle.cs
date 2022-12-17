﻿using Microsoft.UI.Xaml.Media.Animation;
using Shared.Packets;

namespace CardWarsClient;

public class ClientHandle
{
    public static void MakeHandshake(Packet packet)
    {
        var id = packet.ReadInt();
        var message = packet.ReadString();
        Console.WriteLine(message);
        Client.Instance.Id = id;
        ClientSend.WelcomeReceived();
    }

    public static void SendToGame(Packet packet)
    {
        packet.ReadString();
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await Shell.Current.GoToAsync("GamePage");
        });
        //Client.Instance.IsServerReady = true;
    }

    public static void ChooseDeck(Packet packet)
    {
        var action = PacketEncoder.DecodeGameAction(packet);
        throw new NotImplementedException();
    }
}