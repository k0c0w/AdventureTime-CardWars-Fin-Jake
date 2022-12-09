using ConsoleClient;

var r = new Client();
Client.Instance = r;
r.Start();
r.ConnectToServer();
Console.ReadKey();
