using System.Text;
using System.Net.Sockets;
public class ClientInfo
{
    public TcpClient Client { get; }
    public string ClientName { get; }
    public StringBuilder SB = new StringBuilder();

    public ClientInfo (string name, TcpClient client)
    {
        Client = client;
        ClientName = name;
    }
    
}