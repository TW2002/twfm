using GameServerApp.Data;
namespace GameServerApp;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {

            Console.WriteLine("FirstMate GameServer v22.11a - Ready for Combat!");
            Console.WriteLine("Copyright (c) 2022 David McCartney, All rights reserved.");
            Console.WriteLine("");
            Console.WriteLine("Initializing...");
        }
        else
        {
            Console.WriteLine("Hello, World!" + args[0]);
        }

        DatabaseMnager.InitializeDB();

        //Terminal.Proxy proxy = new();
        //proxy.Banner = "FirstMate Proxy v22.30 - Ready for Combat!\n\r" +
                       //"Copyright (c) 2022 David McCartney, All rights reserved.\n\r\n\r";
        //proxy.Start();

        Console.ReadLine();
        Console.WriteLine("Closing GameServer.");
    }
}