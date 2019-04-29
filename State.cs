using System.Text;
using System.Net.Sockets;
public class State
{
    // private TcpClient _client;
    // private StringBuilder _sb = new StringBuilder();
    public TcpClient Client { get; }
    public string ClientName { get; }
    public StringBuilder SB = new StringBuilder();

    public State (string name, TcpClient client)
    {
        Client = client;
        ClientName = name;
    }

    public void Add (byte b)
    {
        SB.Append((char)b);
    }

    public void Send (string text)
    {
        var textToBroadcast = Encoding.ASCII.GetBytes(string.Format("{0}\r\n", text));
    }
}