using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Terminal
{
    public class Console
    {
        const int DEFAULT_COLUMNS = 120;
        const int DEFAULT_LINES = 60;

        public int ConsoleColumns { get; set; }
        public int ConsoleLines { get; set; }

        public Console()
        {
            ConsoleColumns = DEFAULT_COLUMNS;
            ConsoleLines = DEFAULT_LINES;
        }
    }


    public static class Telnet
    {
        private static NetworkStream? stream;

        public static NetworkStream Initialize(Socket socket)
        {
            //stream = Stream;
            stream = new(socket, true);

            // Send Telnet Handshake
            byte[] telnet = {
                    255, 251, 1,    // Telnet (IAC)(Will)(ECHO) - Will Echo
                    255, 251, 3 };  // Telnet (IAC)(Will)(SGA)  - Will Supress Go Ahead
            stream.Write(telnet, 0, telnet.Length);

            ParseResponse();

            return stream;
        }

        /// <summary>
        /// Patrial implimentation of a telnet ParseResponser.
        /// Currently supports Window Size and Terminal Type Only.
        /// </summary>
        /// <param name="stream">NetworkStream to ParseResponse for telnet sequences.</param>
        public static void ParseResponse()
        {
            if (stream == null) return;
            try
            {
                byte[] readBuffer = new byte[1024];
                int readBytes = stream.Read(readBuffer, 0, 1024);

                //String response = Encoding.UTF8.GetString(readBuffer);
                //ansiDetected = Encoding.UTF8.GetString(readBuffer).Contains("\u001B[");


                for (int i = 0; i < readBytes; i++)
                {
                    byte[] debug = Encoding.ASCII.GetBytes(
                        string.Format("{0}:{1} ", (char)readBuffer[i], readBuffer[i]));
                    //todo: echo telnet codes if debug is enabled
                    //stream.Write(debug, 0, debug.Length);

                    // Check for IAC
                    if (readBuffer[i] == 255)
                    {
                        switch (readBuffer[i + 1])
                        {
                            // Handle WILL
                            case 251:
                                // todo:
                                break;

                            // Handle WONT
                            case 252:
                                // todo:
                                break;

                            // Handle DO
                            case 253:
                                // todo:
                                break;

                            // Handle DONT
                            case 254:
                                // todo:
                                break;
                        }
                    }
                }

                //telnetResponse.WindowSizeX = 80;
                //telnetResponse.WindowSizeY = 24;
                //telnetResponse.TerminlaType = "Testing";
            }
            catch (Exception)
            {
                // TODO: Log Telnet ParseResponse error.
            }

            //return telnetResponse;
        }

        public static void GetTerminalType()
        {
            byte[] telnet = {
                    255, 253, 24,   // Telnet (IAC)(DO)(TT)     - Do Terminal Type
                    255, 250, 24, 1, 255, 240, 13, 10 }; //(IAC)(SB)(TT)(1)(IAC)(SE)
            stream.Write(telnet, 0, telnet.Length);

            ParseResponse();
        }

        private static void getWindowSize()
        {
            byte[] telnet = {
                    255, 253, 31 }; // Telnet (IAC)(DO)(WS)    - Do Window Size
            stream.Write(telnet, 0, telnet.Length);

            ParseResponse();
        }


 

    }

}

