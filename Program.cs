using System.Diagnostics;
using System.Threading.Tasks.Dataflow;
using System.Net;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using server.Utilities;

namespace server
{
    class Program
    {
        
        // static TcpListener listener;
        // static Thread serverThread;
        // static Dictionary<int, State> connections = new Dictionary<int, State>();
        public static void Main (string[] args)
        {
            TcpHelper.StartServer(80);
        }
    }
}