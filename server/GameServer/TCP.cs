using System.Net.Sockets;

namespace GameServer;

public class TCP
{
    public static int dataBufferSize = 4096;
    
    public TcpClient Socket { get; set; }

    private readonly int _id;

    private NetworkStream _stream;
    private byte[] _receiveBuffer;

    public TCP(int id) => _id = id;

    public void Connect(TcpClient socket)
    {
        Socket = socket;
        Socket.ReceiveBufferSize = dataBufferSize;
        socket.SendBufferSize = dataBufferSize;
        _stream = socket.GetStream();
        _receiveBuffer = new byte[dataBufferSize];

        _stream.BeginRead(_receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
        //todo: send welcome packet
    }

    private void ReceiveCallback(IAsyncResult result)
    {
        try
        {
            var byteLength = _stream.EndRead(result);
            if (byteLength <= 0)
            {
                //todo: disconnect
                return;
            }

            var data = new byte[byteLength];
            Array.Copy(_receiveBuffer, data, byteLength);
            
            //todo: handle data
            _stream.BeginRead(_receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error receiving TCP data: {ex}");
            //todo: disconnect client and multiple dispatch error
        }
        
    }
}    