using System.Diagnostics;

namespace Proxy;
class Program
{
    static void Main(string[] args)
    {
        const string path = @"C:\Projects\TW2002\twfm\Source\Proxy\Proxy\bin\Debug\net6.0\";

        Console.WriteLine("FirstMate Daemon v2206.30 - Ready for Combat!");
        Console.WriteLine("Copyright (c) 2022 David McCartney, All rights reserved.");
        Console.WriteLine("");
        Console.WriteLine("Initializing...");

        //Process p = Process.Start(path + "FirstMateProxy.exe");
        var p = new System.Diagnostics.Process();
        p.StartInfo.FileName = path + "FirstMateProxy.exe ";
        p.StartInfo.Arguments = "\"MBN - GameA\"";
        p.StartInfo.RedirectStandardOutput = false;
        p.StartInfo.UseShellExecute = false;
        p.StartInfo.CreateNoWindow = false;
        p.Start();
        //p.StandardOutput.ReadToEnd().Dump();

        Terminal.Server server = new();
        server.Banner = "FirstMate Daemon v2206.30 - Ready for Combat!\n\r" +
                        "Copyright (c) 2022 David McCartney, All rights reserved.\n\r\n\r";
        server.Start();


        Console.WriteLine("");
        Console.WriteLine("Waiting for client connections.");
        Console.WriteLine("Press <Enter> to close server.");
        Console.ReadLine();
        Console.WriteLine("Closing server.");
        p.Close();
    }
}
