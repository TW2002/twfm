using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Terminal;

public class Server
{
    const int DEFAULT_PORT = 3000;

    /// <summary>
    /// Occurs when a new client has connected to the server.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public event EventHandler<ConnectedEventArgs> Connected = delegate { };

    /// <summary>
    /// Specifies the IP Address to bind for the TCP listener.
    /// </summary>
    public IPAddress ServerIP { private get; set; }

    /// <summary>
    /// Specifies the Port to bind for the TCP listener.
    /// </summary>
    public int ServerPort { private get; set; }

    /// <summary>
    /// Specifies the initial banner to be displayed to new client connections before initialization.
    /// </summary>
    public string Banner { private get; set; }

    /// <summary>
    /// Specifies the initial banner to be displayed to new client connections before initialization.
    /// </summary>
    public string ServerAddress { private get; set; }

    private bool active;
    private static TcpListener tcpListener;

    /// <summary>
    /// TODO: Comment
    /// </summary>
    public Server()
    {
        active = false;
        ServerIP = IPAddress.Any;
        ServerPort = DEFAULT_PORT;
    }

    /// <summary>
    /// TODO: Comment
    /// </summary>
    public Server(String serverAddress)
    {
        active = false;
        ServerAddress = serverAddress;
        ServerIP = IPAddress.Any;
        ServerPort = DEFAULT_PORT;
    }

    /// <summary>
    /// TODO: Comment
    /// </summary>
    /// <param name="serverIP">Specifies the IP Address to bind for the TCP listener.</param>
    /// <param name="serverPort">Specifies the Port to bind for the TCP listener.</param>
    public Server(IPAddress serverIP, int serverPort)
    {
        active = false;
        ServerIP = serverIP;
        ServerPort = serverPort;
        ServerAddress = null;
    }

    public async void Start()
    {
        if (active == true) return;
        else active = true;

        if (tcpListener != null)
        {
            tcpListener.Start();
            return;
        }

        if (!string.IsNullOrEmpty(ServerAddress))
        {
            try
            {
                // Set the port if specified in serverAddress.
                if (ServerAddress.Contains(":"))
                {
                    String[] s = ServerAddress.Split(':');
                    ServerAddress = s[0];
                    try
                    {
                        ServerPort = int.Parse(s[1]);
                    }
                    catch (Exception)
                    {
                        ServerPort = DEFAULT_PORT;
                    }
                }

                // Resolve host binding from serverAddress
                IPHostEntry ipHost = await Dns.GetHostEntryAsync(ServerAddress);
                ServerIP = ipHost.AddressList[1];
            }
            catch { }
        }

        try
        {
            tcpListener = new TcpListener(ServerIP, ServerPort);
            tcpListener.Start();
        }
        catch (Exception e)
        {
            //Console.WriteLine("\n \n*** Error *** Unable to open TCP listener. Socket returned error message:\n{0}", e.Message);
            //Environment.Exit(1);

        }

        await HandleConnectionsAsync(tcpListener);

        tcpListener.Stop();
    }

    private async Task HandleConnectionsAsync(TcpListener listener)
    {
        while (active)
        {
            // Wait for connections.
            TcpClient client = await listener.AcceptTcpClientAsync();

            var remoteEndPoint = client.Client.RemoteEndPoint as IPEndPoint;

            // Raise client connected event.
            Connected(client, new ConnectedEventArgs(remoteEndPoint.Address.ToString()));

            NetworkStream stream = Telnet.Initialize(client.Client);

            byte[] requestNameData = Encoding.ASCII.GetBytes("Greetings Program...");
            stream.Write(requestNameData, 0, requestNameData.Length);

        }
    }


    public void Pause()
    {
        tcpListener.Stop();
    }

    public void Resume()
    {
        tcpListener.Start();
    }

    public void Stop()
    {
        active = false;
    }
}

public class ConnectedEventArgs : EventArgs
{
    public string Address { get; private set; }

    public ConnectedEventArgs(string address)
    {
        Address = address;
    }
}
