using GameServer;

Console.Title = "DS Server";
Server.Start(2, 26950);
//todo: освободить поток передалть на await 
//todo: логика отключения клиента
Console.ReadKey();