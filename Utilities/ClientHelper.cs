using System.Diagnostics;
using System.Threading.Tasks.Dataflow;
using System.Net;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Utilities
{
    class ClientHelper
    {
        static TcpListener listener;
        static Thread serverThread;
        static Dictionary<int, State> connections = new Dictionary<int, State>();
        public static void yadda (string[] args)
        {
            // int port = 13000;
            // IPAddress localAddress = IPAddress.Parse("127.0.0.1");
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
                clientThread.Start(client);            
            }
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
                    //determine safe way to abort thread                    
                }

                clientName = Receive(client);
                done = true;

                if (done)
                {
                    foreach (var connection in connections)
                    {
                        var state = connection.Value;
                        if (state.ClientName == clientName)
                        {
                            getClientName = Encoding.ASCII.GetBytes("That name has already been registered. Please enter another: ");
                            client.GetStream().Write(getClientName, 0, getClientName.Length);
                            done = false;
                        }
                    }
                }
            } while (!done);

            connections.Add(currentThread.ManagedThreadId, new State(clientName, client));
            Console.WriteLine("\tTotal connections: {0}", connections.Count);
            Broadcast(string.Format("~~~ {0} has arrived ~~~", clientName));

            do
            {
                var textFromClient = Receive(client);
                
                if (textFromClient == "/quit")
                {
                    Broadcast(string.Format("~~~ {0} has departed ~~~", clientName));
                    connections.Remove(currentThread.ManagedThreadId);
                    Console.WriteLine("\tTotal Connections: {0}", connections.Count);
                    break;
                }

                if (!client.Connected)
                {
                    break;
                }
                Broadcast(string.Format("{0}> {1}", clientName, textFromClient));
            } while (true);

            Console.WriteLine("Client (Thread {0}) Terminated.", currentThread.ManagedThreadId);
            client.Close();
            //determine safe way to abort thread
        }

        private static string Receive (TcpClient client)
        {
            var sb = new StringBuilder();
            do
            {
                if (client.Available > 0)
                {
                    while (client.Available > 0)
                    {
                        var ch = (char)client.GetStream().ReadByte();
                        
                        if (ch == '\r')
                        {
                            continue;
                        }

                        if (ch == '\n')
                        {
                            return sb.ToString();
                        }
                        sb.Append(ch);
                    }
                }
                Thread.Sleep(100);
            } while (true);
        }

        private static void Broadcast (string text)
        {
            Console.WriteLine(text);
            foreach (var client in connections)
            {
                if (client.Key != Thread.CurrentThread.ManagedThreadId)
                {
                    State state = client.Value;
                    state.Send(text);
                }
            }
        }
    }
}