using GameServer;

Console.Title = "DS Server";

//todo: освободить поток передалть на await или придумать как высвободить поток
Server.Start(2, 26950);
Console.ReadKey();