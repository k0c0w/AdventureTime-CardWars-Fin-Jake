using System.Net.Sockets;
using Shared.Packets;

namespace CardWarsClient;

public class Client
{
    public static Client Instance { get; set; }
    public static int dataBufferSize = 4096;
    public string ip = "127.0.0.1";
    public int port = 26950;
    public int Id = 1;
    bool isServerReady = false;
    public bool IsServerReady
    {
        get
        {
            return isServerReady;
        }
        set
        {
            isServerReady = value;
            OnChangedServerReadiness();
        }
    }
    public TCP Tcp { get; private set; }


    private delegate void PacketHandler(Packet packet);

    private static Dictionary<PacketId, Dictionary<int, PacketHandler>> packetHandlers;

    public void Start()
    {
        Tcp = new TCP();
    }

    public void ConnectToServer()
    {
        InitializeData();
        Tcp.Conncet();
    }
    
    public class TCP
    {
        public TcpClient socket { get; private set; }
        private Packet receivedData;
        private NetworkStream stream;
        private byte[] receiveBuffer;

        public void Conncet()
        {
            socket = new TcpClient
            {
                ReceiveBufferSize = dataBufferSize,
                SendBufferSize = dataBufferSize
            };

            receiveBuffer = new byte[dataBufferSize];
            socket.BeginConnect(Instance.ip, Instance.port, ConnectCallback, socket);
        }

        private void ConnectCallback(IAsyncResult result)
        {
            socket.EndConnect(result);

            if (!socket.Connected)
            {
                return;
            }

            stream = socket.GetStream();

            receivedData = new Packet();
            
            stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
        }

        private void ReceiveCallback(IAsyncResult result)
        {
            try
            {
                var byteLength = stream.EndRead(result);
                if (byteLength <= 0)
                {
                    //todo: dissconnect
                    return;
                }

                var data = new byte[byteLength];
                Array.Copy(receiveBuffer, data, byteLength);
                
                //to handle splited packets we dont need always reset packet
                receivedData.Reset(HandleData(data));
                
                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
            }
            catch
            {
                //todo: dissconnect
            }
        }

        public void SendData(Packet packet)
        {
            try
            {
                if (socket != null)
                    stream.BeginWrite(packet.ToArray(), 0, packet.Length, null, null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private bool HandleData(byte[] data)
        {
            var packetLength = 0;
            
            receivedData.SetBytes(data);

            if (receivedData.UnreadLength >= 4)
            {
                packetLength = receivedData.ReadInt();
                if (packetLength <= 0)
                    return true;
            }

            while (0 < packetLength && packetLength <= receivedData.UnreadLength)
            {
                var packetsBytes = receivedData.ReadBytes(packetLength);
                var packet = new Packet(packetsBytes);
                var packetId = packet.ReadInt();
                if (packetId == (int)PacketId.ServerPacket)
                {
                    var packetSubId = packet.ReadInt();
                    packetHandlers[PacketId.ServerPacket][packetSubId](packet);
                } 
                else if(packetId == (int)PacketId.GameActionPacket)
                {
                    //PacketEncoder внутри считывает SubId, поэтому не двигаем указатель, 
                    var packetSubId = packet.ReadInt(false);
                    packetHandlers[PacketId.GameActionPacket][packetSubId](packet);        
                }

                packetLength = 0;
                if (receivedData.UnreadLength >= 4)
                {
                    packetLength = receivedData.ReadInt();
                    if (packetLength <= 0)
                        return true;
                }

                //throw new ArgumentException($"Unsupported packet {packetId}");
                return true;
            }

            if (packetLength <= 1)
                return true;

            return false;
        }
    }

    public static void InitializeData()
    {
        packetHandlers = new Dictionary<PacketId, Dictionary<int, PacketHandler>>()
        {
            { 
                PacketId.ServerPacket, new Dictionary<int, PacketHandler>()
                { { (int)ServerPacket.Welcome, ClientHandle.MakeHandshake } }
            },
            {
                PacketId.GameActionPacket, new Dictionary<int, PacketHandler>()
                {
                    { (int)GameActionPacket.GameStart, ClientHandle.SendToGame }, 
                }
            }
        };
        var multipleDispatch = new PacketHandler(ClientHandle.Dispatch);
        var gameActionHandlers = packetHandlers[PacketId.GameActionPacket];
        for(var i = 1; i < (int)GameActionPacket.GameEnd; i++)
        {
            gameActionHandlers.Add(i, multipleDispatch);
        }

    }

    private async void OnChangedServerReadiness()
    {
        if (isServerReady) await Shell.Current.GoToAsync("GamePage");
    }
}