using System;
using System.Net.Sockets;
public class ClientInfo
{
    public TcpClient Client { get; }
    public string ClientName { get; }
    public ClientInfo (string name, TcpClient client)
    {
        Client = client;
        ClientName = name;
    }    
}