using FirstMate;
using Microsoft.Extensions.Hosting;

namespace Daemon
{
    public class Program
    {
        static void Main(string[] args) => Server.Start(args);
    }
}
