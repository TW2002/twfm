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

    const string prompt = "Server Menu - Press <Enter> to Quit <?=Help> [Q]: ";

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

            Write(stream, Banner);
            Write(stream, prompt);

            Receive(client.Client);
        }
    }

    private void Write(NetworkStream stream, string s)
    {
        byte[] requestNameData = Encoding.ASCII.GetBytes(s);
        stream.Write(requestNameData, 0, requestNameData.Length);
    }


    private void Receive(Socket client)
    {
        try
        {
            // Create the state object.
            StateObject state = new StateObject();
            state.workSocket = client;

            // Begin receiving the data from the remote device.
            client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReceiveCallback), state);
        }
        catch (Exception e)
        {
            //Console.WriteLine(e.ToString());
        }
    }

    private void ReceiveCallback(IAsyncResult ar)
    {
        try
        {
            // Retrieve the state object and the client socket 
            // from the asynchronous state object.
            StateObject? state = (StateObject)ar.AsyncState;
            Socket client = state.workSocket;

            // Read data from the remote device.
            int bytesRead = client.EndReceive(ar);

            if (bytesRead > 0)
            {
                // There might be more data, so store the data received so far.
                //state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));
                string s = Encoding.ASCII.GetString(state.buffer, 0, bytesRead);

                // Echo the text received back to the client.
                NetworkStream stream = new(client, true);
                Write(stream, s);

                // Process each character.
                foreach(char c in s)
                {
                    switch (char.ToLower(c))
                    {
                        case (char)13:
                        case 'q':
                            break;

                        case 'l':
                            DisplayGames(stream);
                            break;

                        case '?':
                            DisplayHelp(stream);
                            break;

                    }

                }

                // Get the rest of the data.
                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReceiveCallback), state);
            }
            else
            {
                //TODO: Handle client disconnected.

                // All the data has arrived; put it in response.
                if (state.sb.Length > 1)
                {
                    //response = state.sb.ToString();
                }
                // Signal that all bytes have been received.
                //receiveDone.Set();
            }
        }
        catch (Exception e)
        {
            //Console.WriteLine(e.ToString());
        }
    }

    private void DisplayHelp(NetworkStream stream)
    {
        Write(stream, "\n\r\n\rAvailable Commands:\n\r\n\r" +
                      "<L> List available games\n\r" +
                      "<Q> Quit (Disconnect)\n\r\n\r" + prompt);
    }

    private void DisplayGames(NetworkStream stream)
    {
        Write(stream, "\n\r\n\rCurrrent Games:\n\r\n\r" +
                      "MBN - GameZ (2301) - Connected\n\r" +
                      "Ice9 - GameA (2302) - Disconnected\n\r\n\r" + prompt);
    }

    // State object for receiving data from remote device.
    public class StateObject
    {
        // Client socket.
        public Socket workSocket = null;
        // Size of receive buffer.
        public const int BufferSize = 1024;
        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];
        // Received data string.
        public StringBuilder sb = new StringBuilder();
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
