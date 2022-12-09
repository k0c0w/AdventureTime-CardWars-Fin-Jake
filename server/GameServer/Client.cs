using System.Net;
using System.Net.Sockets;

namespace GameServer;

public class Client
{
    public const int DataBufferSize = 4096;
    public int Id { get;}
    public Player player;
    public TCP Tcp { get; }

    public Client(int _clientId)
    {
        Id = _clientId;
        Tcp = new TCP(Id);
    }
    public class TCP
    {
        public TcpClient socket;

        private readonly int id;
        private NetworkStream stream;
        private Packet receivedData;
        private byte[] receiveBuffer;

        public TCP(int _id)
        {
            id = _id;
        }

        public void Connect(TcpClient _socket)
        {
            socket = _socket;
            socket.ReceiveBufferSize = DataBufferSize;
            socket.SendBufferSize = DataBufferSize;

            stream = socket.GetStream();

            receivedData = new Packet();
            receiveBuffer = new byte[DataBufferSize];

            stream.BeginRead(receiveBuffer, 0, DataBufferSize, ReceiveCallback, null);

            ServerSend.MakeHandshake(id, "Welcome to the Server!");
        }

        public void SendData(Packet packet)
        {
            try
            {
                if(socket != null)
                {
                    stream.BeginWrite(packet.ToArray(),0,packet.Length, null, null);
                }
            }
            catch (Exception e)
            {
                 Console.WriteLine($"Error sending data to player {id} via TCP: {e.ToString()}");
            }
        }

        private void ReceiveCallback(IAsyncResult _result)
        {
            try
            {
                int _byteLength = stream.EndRead(_result);
                if( _byteLength <= 0)
                {
                    Disconnect();
                    return;
                }

                var _data = new byte[_byteLength];
                Array.Copy(receiveBuffer, _data, _byteLength);

                receivedData.Reset(HandleData(_data));
                stream.BeginRead(receiveBuffer,0,DataBufferSize, ReceiveCallback, null);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error receiving TCP data: {e.ToString()}");
                Disconnect();
            }
        }

        private bool HandleData(byte[] _data)
        {
            var packetLength = 0;

            receivedData.SetBytes(_data);
            if (receivedData.UnreadLength >= 4)
            {
                packetLength = receivedData.ReadInt();
                if (packetLength <= 0)
                {
                    return true;
                }
            }

            while (0 < packetLength && packetLength <= receivedData.UnreadLength)
            {
                var packetsBytes = receivedData.ReadBytes(packetLength);
                using var packet = new Packet(packetsBytes);
                var packetType = packet.ReadInt();

                Server.packetHandlers[packetType](id, packet);

                packetLength = 0;
                if (receivedData.UnreadLength >= 4)
                {
                    packetLength = receivedData.ReadInt();
                    if (packetLength <= 0)
                        return true;
                }
            }


            if (packetLength < 1) return true;
            return false;
        }
        
        private void Disconnect()
        {
            socket.Close();
            socket = null!;
        }
    }
}