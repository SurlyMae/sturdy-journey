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

        private static void Listen ()
        {
            listener.Start();
            Console.WriteLine("Server has started.");

            while (true)
            {
                Console.WriteLine("Server is waiting...");
                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine("Server waited.");
                
                Thread clientThread = new Thread(new ParameterizedThreadStart(ManageClient));
                clientThread.Start(client);            }
        }

        private static void ManageClient (object oClient)
        {
            TcpClient client = (TcpClient)oClient;
            var currentThread = Thread.CurrentThread;
            //Console.WriteLine("Client (Thread {0}) Connected.", Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine("Client (Thread {0}) Connected.", currentThread.ManagedThreadId);

            byte[] getClientName = Encoding.ASCII.GetBytes("Enter name: ");
            client.GetStream().Write(getClientName, 0, getClientName.Length);

            var clientName = "";
            var done = false;
            
            do
            {
                if (!client.Connected)
                {
                    Console.WriteLine("Client (Thread {0}) Terminated.", currentThread.ManagedThreadId);
                    client.Close();
                    currentThread.Abort();
                }

                clientName = Receive(client);
                done = true;

                if (done)
                {
                    foreach (var connection in connections)
                    {
                        var state = connection.Value;
                        if (state.Name == clientName)
                        {
                            getClientName = Encoding.ASCII.GetBytes("That name has already been registered. Please enter another: ");
                            client.GetStream().Write(getClientName, 0, getClientName.Length);
                            done = false;
                        }
                    }
                }
            } while (!done);
        }
    }
}