using server.Utilities;

namespace server
{
    class Program
    {    
        public static void Main (string[] args)
        {
            TcpHelper.StartServer(80);
        }
    }
}