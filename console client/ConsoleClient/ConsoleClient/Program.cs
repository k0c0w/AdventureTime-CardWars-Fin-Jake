using ConsoleClient;

var r = new Client();
Client.Instance = r;
r.Start();
r.ConnectToServer(); 
Console.ReadLine();


var readyPacket = new Packet((int)PacketId.ClientPacket);

readyPacket.Write((int)ClientPacket.ReadyForGame);
readyPacket.Write(true);
readyPacket.WriteLength();
r.Tcp.SendData(readyPacket);
Console.WriteLine("готов");

Console.ReadKey();