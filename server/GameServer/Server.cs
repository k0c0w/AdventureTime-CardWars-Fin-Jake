using System.Net;
using System.Net.Sockets;
using GameKernel;
using Shared.Packets;

namespace GameServer
{
    internal class Server
    {
        public static Game CurrentGame { get; set; }
        public static int MaxPlayers { get; private set; }
        public static int Port { get; private set; }

        public static Dictionary<int, Client> Clients;
        public delegate void PacketHandler(int _fromClient, Packet _packet);
        public static Dictionary<PacketId, Dictionary<int, PacketHandler>> PacketHandlers;

        private static TcpListener tcpListener;

        public static void Start(int _maxPlayers, int _port)
        {
            MaxPlayers = _maxPlayers;
            Port = _port;
            InitializeServerData();

            Console.WriteLine("Starting server...........");

            tcpListener = new TcpListener(IPAddress.Any, Port);
            tcpListener.Start();
            tcpListener.BeginAcceptTcpClient(TCPConnectCallback, null);
            
            Console.WriteLine($"Server started on port #{Port}");
        }

        private static void TCPConnectCallback(IAsyncResult _result)
        {
            var client = tcpListener.EndAcceptTcpClient(_result);
            tcpListener.BeginAcceptTcpClient(TCPConnectCallback, null);
            Console.WriteLine($"Incoming connection from {client.Client.RemoteEndPoint}...");

            for (var i = 1; i <= MaxPlayers; i++)
            {
                if(Clients[i].Tcp.Socket == null)
                {
                    Clients[i].Tcp.Connect(client);
                    return;
                }
            }

            Console.WriteLine($"{client.Client.RemoteEndPoint} failed to connect: server full!");
        }
        
        private static void InitializeServerData()
        {
            Clients = new Dictionary<int, Client>();
            for(int i = 1; i <= MaxPlayers; i++)
            {
                Clients.Add(i, new Client(i));
            }

            PacketHandlers = new Dictionary<PacketId, Dictionary<int, PacketHandler>>()
            {
                { 
                    PacketId.ClientPacket, new Dictionary<int, PacketHandler>()
                    {
                        { (int)ClientPacket.WelcomeReceived, ServerHandler.WelcomeReceived },
                        { (int)ClientPacket.ReadyForGame, ServerHandler.ChangeUserReadiness },
                    }
                },
                { PacketId.GameActionPacket, new Dictionary<int, PacketHandler>() },
            };
            var handler = new PacketHandler(GameActionHandler.ApplyToGame);
            var gameActions = PacketHandlers[PacketId.GameActionPacket];
            for(var action = 0; action <= (int)GameActionPacket.GameEnd; action++)
                gameActions.Add(action, handler);

            Console.WriteLine("Initialized packets.");
        }
    }
}
