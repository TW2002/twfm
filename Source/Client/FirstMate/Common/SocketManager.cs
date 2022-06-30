using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal;

namespace FirstMate;

public class SocketManager
{
    const int DEFAULT_PROCY_PORT = 3000;

    static public List<Client> ActiveSockets { get; private set; }
    static public Client ProxySocket { get; private set; }

    static public Client CreateProxy()
    {
        ProxySocket = new("", DEFAULT_PROCY_PORT);

        TrackSocket(ProxySocket);
        return ProxySocket;
    }

    static public Client CreateSocket(string address, int port)
    {
        Client socket = new(address, port);

        TrackSocket(socket);
        return socket;
    }

    static public void TrackSocket(Client socket)
    {
        if (ActiveSockets == null) ActiveSockets = new();

        //window.Closed += (sender, args) => {
        //    ActiveWindows.Remove(window);
        //};
        ActiveSockets.Add(socket);
    }

}

//public class Socket
//{
//    TelnetSocket tSocket;

//    public Socket(string address, int port)
//    {
//        tSocket = new(address, port);
//    }

//    public void Connect()
//    {
//        tSocket.Connect();
//    }
//}



