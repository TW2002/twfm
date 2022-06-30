

Console.WriteLine("FirstMate Proxy v2106.30");
Console.WriteLine("Initializing...");

Proxy.Server server = new();
server.Start();

Console.WriteLine("Waiting for client connections.");
Console.WriteLine("Press <Enter> to close server.");
Console.ReadLine();
