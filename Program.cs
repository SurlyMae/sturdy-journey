using System.Net;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace server
{
    class Program
    {
        static TcpListener listener;
        static Thread serverThread;

        static Dictionary<int, State> connections = new Dictionary<int, State>();
        public static void Main (string[] args)
        {
            listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 80);
            serverThread = new Thread(new ThreadStart(Listen));
            serverThread.Start();
        }
    }
}