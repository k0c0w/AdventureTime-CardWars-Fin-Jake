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

            ServerSend.Welcome(id, "Welcome to the Server!");
        }

        public void SendData(Packet _packet)
        {
            try
            {
                if(socket != null)
                {
                    stream.BeginWrite(_packet.ToArray(),0,_packet.Length, null, null);
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
                //TODO: disconnect
            }
        }

        private bool HandleData(byte[] _data)
        {
            var _packetLength = 0;

            receivedData.SetBytes(_data);
            if (receivedData.UnreadLength >= 4)
            {
                _packetLength = receivedData.ReadInt();
                if (_packetLength <= 0)
                {
                    return true;
                }
            }

            while (_packetLength > 0 && _packetLength <= receivedData.UnreadLength)
            {
                var _packetBytes = receivedData.ReadBytes(_packetLength);

                _packetLength = 0;
                if (receivedData.UnreadLength >= 4)
                {
                    _packetLength = receivedData.ReadInt();
                    if (_packetLength <= 0)
                    {
                        return true;
                    }
                }
            }

            if (_packetLength < 1) return true;
            return false;
        }
    }

    public void SendIntoGame(string _playerName)
    {
        player = new Player(Id, _playerName);

        foreach(var _client in Server.Clients.Values)
        {
            if(_client.player != null)
            {
                if(_client.Id != Id)
                {
                    ServerSend.SpawnPlayer(Id,_client.player);
                }
            }
        }

        foreach (var _client in Server.Clients.Values)
        {
            if (_client.player != null)
            {
                ServerSend.SpawnPlayer(_client.Id, player);
            }
        }
    }
}