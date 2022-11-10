using System;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;


namespace Terminal;

public class Client
{
    //public event EventHandler Connected;
    public event EventHandler Closed;
    public event EventHandler Failed;
    public event EventHandler<ReceivedEventArgs> Received;

    public bool Connected { get; private set; }
    public string? Address { get; private set; }
    public int Port { get; private set; }

    private static Socket? socket;

    //    Why wouldn't a new SemaphoreSlim(1) work, WaitOne() is WaitAsync() and Set() becomes Release() – 
    //Scott Chamberlain
    // Nov 2, 2016 at 21:40
    private static SemaphoreSlim connectDone = new(0);

    //private static ManualResetEvent connectDone = new(false);


    // ManualResetEvent instances signal async completion.
    //private static ManualResetEvent sendDone = new(false);
    //private static ManualResetEvent receiveDone = new(false);

    public Client(string? address = null, int port = 0)
    {
        Address = address;
        Port = port;

        Connected = false;

    }

    public async void Connect()
    {
        await ConnectAsync();

        // TODO: Use await instead of Mutex
        //connectDone.WaitOne();
    }

    public async Task ConnectAsync()
    {
        IPHostEntry hostEntry;

        try
        {

            // Create an sndpoint from Address
            if (String.IsNullOrEmpty(Address))
            {
                //hostEntry = Dns.GetHostEntry(Dns.GetHostName(), AddressFamily.InterNetwork);
                hostEntry = Dns.GetHostEntry("localhost", AddressFamily.InterNetwork);
            }
            else
            {
                hostEntry = Dns.GetHostEntry(Address, AddressFamily.InterNetwork);
            }
            IPAddress ipAddress = hostEntry.AddressList[0];
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, Port);

            // Creation TCP/IP Socket using
            // Socket Class Constructor
            socket = new Socket(ipAddress.AddressFamily,
                         SocketType.Stream, ProtocolType.Tcp);

            // Connect to the remote endpoint.
            socket.BeginConnect(remoteEP,
            new AsyncCallback(ConnectCallback), socket);

            //todo: make this function asynce
            //ValueTask<TcpClient> task = new();

            await connectDone.WaitAsync();

    //SocketAsyncEventArgs connectArgs = newocketAsyncEventArgs();

            //connectArgs.UserToken = cliencSocket;
            //connectArgs.RemoteEndPoint = remoteEP;
            //connectArgs.Completed += new EventHandler<socketasynceventargs> (ConnectCallback);

            //socket.ConnectAsync(connectArgs);

            // TODO this should have an await to signal when the connectin is completed
            //connectDone.WaitOne();


            // Send test data to the remote device.
            //Send(socket, "This is a test<EOF>");
            //Telnet.Initialize(socket);
            //sendDone.WaitOne();

            // Receive the response from the remote device.
            //Receive(socket);
            //receiveDone.WaitOne();
}
        catch (Exception e)
        {
            throw new NotImplementedException();

        }
    }

    private void ConnectCallback(IAsyncResult ar)
    {
        try
        {
            // Retrieve the socket from the state object.
            Socket? socket = ar.AsyncState as Socket;

            // Verify we received a client socket
            if (socket == null) return;

            // Complete the connection.
            socket.EndConnect(ar);

            //string state = ar.AsyncState.ToString();
            //if (socket.Connected)

            // We are connected. Set connected falg.
            Connected = true;

            // Initialize the client connection.
            Telnet.Initialize(socket);

            // Begin receiveing data.
            Receive(socket);

            //Console.WriteLine("Socket connected to {0}",
            //client.RemoteEndPoint.ToString());

            // Signal that the connection has been made.
            connectDone.Release();
        }
        catch (Exception e)
        {
            if (Failed != null)
            {
                Failed.Invoke(this, EventArgs.Empty);
            }
        }
    }

    private static void Receive(Socket client)
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

    private static void ReceiveCallback(IAsyncResult ar)
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
                state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

                // Get the rest of the data.
                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReceiveCallback), state);
            }
            else
            {
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

public class ReceivedEventArgs : EventArgs
{
    public string Response { get; private set; }

    public ReceivedEventArgs(string response)
    {
        Response = response;
    }
}

//    public void oldstuff() 
//    { 

//                // Connect Socket to the remote endpoint.
//                socket.Connect(localEndPoint);

//                // Check if Connection Failed
//                if (socket == null)
//                {
//                    // Raise Connection failed event.
//                    Failed.Invoke(this, new EventArgs());
//                    return;
//                }

//                // Connection was successful, update connected status.
//                Connected = true;

//                // Invoke the Connected event.
//                //if (Connected != null)
//                //{
//                //    Connected.Invoke(this, new EventArgs());
//                //}

//                // Initialize the telner connection
//                Telnet.Initialize(socket);


//                SocketAsyncEventArgs e = new();
//                e.Completed += DataReceived();
//                socket.ReceiveAsync(e);

//                //byte[] readBuffer = new byte[1024];
//                //string data;
//                //do
//                //{
//                //    data = "";
//                //    int readBytes;
//                //    do
//                //    {
//                //        readBytes = socket.Receive(readBuffer, readBuffer.Length, 0);
//                //        data = data + Encoding.ASCII.GetString(readBuffer, 0, readBytes);
//                //    }
//                //    while (readBytes > 0);

//                //    if (Received != null)
//                //    {
//                //        Received.Invoke(this, new ReceivedEventArgs(data));
//                //    }

//                //    //System.t

//                //}
//                //while (socket.Connected);
//            }
//        }
//        catch (Exception e)
//        {
//            if (Closed != null)
//            {
//                Closed.Invoke(this, new EventArgs());
//            }
//        }
//    }

//    private EventHandler<SocketAsyncEventArgs> DataReceived()
//    {
//        throw new NotImplementedException();
//    }
//}



