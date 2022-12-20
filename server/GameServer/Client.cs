using System.Net;
using System.Net.Sockets;
using Shared.Packets;

namespace GameServer;

public class Client
{
    public const int DataBufferSize = 4096;
    public int Id { get;}
    
    public Player? Player;
    public TCP Tcp { get; }

    public bool IsWaitingForGame => Player != null;

    public Client(int clientId)
    {
        Id = clientId;
        Tcp = new TCP(Id);
    }
    public class TCP
    {
        public TcpClient Socket { get; private set; }

        private readonly int _id;
        private NetworkStream _stream;
        private Packet _receivedData;
        private byte[] _receiveBuffer;

        public TCP(int id) => _id = id;

        public void Connect(TcpClient socket)
        {
            Socket = socket;
            Socket.ReceiveBufferSize = DataBufferSize;
            Socket.SendBufferSize = DataBufferSize;

            _stream = Socket.GetStream();

            _receivedData = new Packet();
            _receiveBuffer = new byte[DataBufferSize];

            _stream.BeginRead(_receiveBuffer, 0, DataBufferSize, ReceiveCallback, null);

            ServerSend.MakeHandshake(_id, "Welcome to the Server!");
        }

        public void SendData(Packet packet)
        {
            try
            {
                if(Socket != null)
                    _stream.BeginWrite(packet.ToArray(),0,packet.Length, null, null);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error sending data to player {_id} via TCP: {e.ToString()}");
                Disconnect();
            }
        }

        private void ReceiveCallback(IAsyncResult result)
        {
            try
            {
                var byteLength = _stream.EndRead(result);
                if( byteLength <= 0)
                {
                    Disconnect();
                    return;
                }

                var data = new byte[byteLength];
                Array.Copy(_receiveBuffer, data, byteLength);

                _receivedData.Reset(HandleData(data));
                _stream.BeginRead(_receiveBuffer,0,DataBufferSize, ReceiveCallback, null);
            }
            catch (Exception e)
            {
                Disconnect();
            }
        }
        
        private bool HandleData(byte[] data)
        {
            var packetLength = 0;

            _receivedData.SetBytes(data);
            if (_receivedData.UnreadLength >= 4)
            {
                packetLength = _receivedData.ReadInt();
                if (packetLength <= 0)
                    return true;
            }

            while (0 < packetLength && packetLength <= _receivedData.UnreadLength)
            {
                var packetsBytes = _receivedData.ReadBytes(packetLength);
                using var packet = new Packet(packetsBytes);
                var packetId = packet.ReadInt();
                if ((PacketId)packetId == PacketId.GameActionPacket)
                {
                    var subId = packet.ReadInt(false);
                    Server.PacketHandlers[PacketId.GameActionPacket][subId](_id, packet);
                }
                else
                {
                    var packetSubId = packet.ReadInt();
                    Server.PacketHandlers[(PacketId)packetId][packetSubId](_id, packet);
                }
                packetLength = 0;
                if (_receivedData.UnreadLength >= 4)
                {
                    packetLength = _receivedData.ReadInt();
                    if (packetLength <= 0)
                        return true;
                }
            }
            
            return packetLength < 1;
        }
        
        private void Disconnect()
        {
            if (Server.CurrentGame != null)
            {
                //todo: winner
                Server.CurrentGame = null;
                Console.WriteLine("Game destroyed");
            }
            Server.Clients[_id].Player = null;
            Socket.Close();
            Socket = null!;
            Console.WriteLine($"User {_id} disconnect");
        }
    }
}