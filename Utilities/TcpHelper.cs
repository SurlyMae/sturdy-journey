using System;
using System.Diagnostics;
using System.Net;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using static System.Console;
using server;

namespace server.Utilities
{
    class TcpHelper
    {
        private static TcpListener listener { get; set; }
        private static Thread serverThread { get; set; }
        private static Dictionary<int, ClientInfo> connections = new Dictionary<int, ClientInfo>();

        public static void StartServer (int port)
        {
            IPAddress localAddress = GetIPAddress();
            listener = new TcpListener(localAddress, port);
            serverThread = new Thread(Listen);
            serverThread.Start();
        }

        private static IPAddress GetIPAddress ()
        {
            return IPAddress.Parse("127.0.0.1");
        }

        private static void Listen ()
        {
            listener.Start();
            Broadcast("Server has started.");
            while(true)
            {
                Broadcast("Server is waiting...");
                TcpClient client = listener.AcceptTcpClient();                
                if (client.Connected)
                {
                    StartClientThread(client);
                }                        
            }
        }

        private static void StartClientThread (TcpClient client)
        {
            Thread clientThread = new Thread(ManageClient);
            clientThread.Start(client);    
        }

        private static void ManageClient (object oClient)
        {
            //still need to refactor this method, it has way too much going on
            TcpClient client = (TcpClient)oClient;                        
            var threadId = Thread.CurrentThread.ManagedThreadId;
            Encoding ascii = Encoding.ASCII;
            
            Broadcast($"Client connected on thread {threadId}.");

            byte[] bytes = ascii.GetBytes("Enter name: ");
            client.GetStream().Write(bytes, 0, bytes.Length);

            var clientName = "";
            var done = false;
            
            do
            {
                if (!client.Connected)
                {
                    Broadcast($"Client disconnected on thread {threadId}.");
                    client.Close();
                    //determine safe way to abort thread                    
                }

                clientName = GetInput(client);
                done = true;

                if (done)
                {
                    foreach (var connection in connections)
                    {
                        var clientInfo = connection.Value;
                        
                        if (clientInfo.ClientName == clientName)
                        {
                            bytes = ascii.GetBytes("That name has already been registered. Please enter another: ");
                            client.GetStream().Write(bytes, 0, bytes.Length);
                            done = false;
                        }
                    }
                }
            } while (!done);

            connections.Add(threadId, new ClientInfo(clientName, client));
            Broadcast($"\tTotal connections: {connections.Count}");
            Broadcast($"~~~ {clientName} has arrived ~~~");

            do
            {
                var textFromClient = GetInput(client);
                var timestamp = DateTime.Now;
                
                if (textFromClient == "/quit")
                {
                    Broadcast($"~~~ {clientName} has departed ~~~");
                    connections.Remove(threadId);
                    Broadcast($"\tTotal connections: {connections.Count}");
                    break;
                }

                if (!client.Connected)
                {
                    break;
                }
                Broadcast($"{clientName} @ {timestamp}> {textFromClient}");
            } while (true);

            Broadcast($"Client disconnected at thread {threadId}.");
            client.Close();
            //determine safe way to abort thread
        }

        private static string GetInput (TcpClient client)
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
            WriteLine(text);
        }
    }
}