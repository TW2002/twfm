using Configuration.Data;
using Terminal;

namespace GameServerApp;

class Program
{
    static void Main(string[] args)
    {
        var config = DatabaseMnager.LoadConfig("GameServer");

        if (args.Length > 0)
        { 
            Console.WriteLine("Command Line:" + args[0]);
        }

        Console.Write($"{config.Banner}\n\rInitializing...");

        Server server = new("localhost:2300");
        server.Start();

        Console.ReadLine();
        Console.WriteLine("Closing GameServer.");
    }
}