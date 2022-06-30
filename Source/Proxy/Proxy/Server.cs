using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal;

namespace Proxy
{
    public class Server
    {
        Terminal.Server server;

        public Server()
        {
            server = new();
        }

        public void Start()
        {

            server.Start();
        }
    }
}
