using System;

namespace server
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpHelper.StartServer(5789);
            TcpHelper.Listen();
        }
    }
}
