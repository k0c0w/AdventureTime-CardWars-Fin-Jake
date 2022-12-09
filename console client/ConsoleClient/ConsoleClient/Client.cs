using System.Net.Sockets;
using System.Text;

namespace ConsoleClient;

public class Client
{
    public static Client Instance { get; set; }
    public static int dataBufferSize = 4096;
    public string ip = "127.0.0.1";
    public int port = 26950;
    public int Id = 1;
    public TCP Tcp { get; private set; }


    private delegate void PacketHandler(Packet packet);

    private static Dictionary<int, PacketHandler> packetHandlers;

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
                using var packet = new Packet(packetsBytes);
                var packetType = packet.ReadInt();

                packetHandlers[packetType](packet);

                packetLength = 0;
                if (receivedData.UnreadLength >= 4)
                {
                    packetLength = receivedData.ReadInt();
                    if (packetLength <= 0)
                        return true;
                }
            }

            if (packetLength <= 1)
                return true;

            return false;
        }
    }

    public static void InitializeData()
    {
        packetHandlers = new Dictionary<int, PacketHandler>()
            { { (int)ServerPackets.Welcome, ClientHandle.MakeHandshake } };
    }
}

public enum ServerPackets
{
    Welcome = 1
}

public enum ClientPackets
{
    WelcomeReceived = 1
}