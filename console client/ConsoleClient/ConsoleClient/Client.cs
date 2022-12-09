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

    
    public void Start()
    {
        Tcp = new TCP();
    }

    public void ConnectToServer()
    {
        Tcp.Conncet();
        
    }
    
    public class TCP
    {
        public TcpClient socket { get; private set; }
 
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
                Console.WriteLine(Encoding.UTF8.GetString(data));
                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
            }
            catch
            {
                //todo: dissconnect
            }
        }
    }
}