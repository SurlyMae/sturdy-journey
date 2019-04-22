using System;  
using System.Net;  
using System.Text;  
using System.Net.Sockets;

namespace server
{
    class TcpHelper
    {
        private static TcpListener listener { get; set; }
        private static bool accept { get; set; } = false;

        public static void StartServer(int port)
        {
            IPAddress address = IPAddress.Parse("127.0.0.1");
            listener = new TcpListener(address, port);

            listener.Start();
            accept = true;

            Console.WriteLine($"Server started. Listening to TCP client on {port}");
        }
    }
}