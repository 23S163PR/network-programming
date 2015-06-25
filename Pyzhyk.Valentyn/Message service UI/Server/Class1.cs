using System.Net.Sockets;

namespace Server
{
    public class Client
    {
        public Client(Socket socketRead, Socket socketWrite)
        {
            SocketRead = socketRead;
            SocketWrite = socketWrite;
        }

        public Socket SocketRead { get; private set; }
        public Socket SocketWrite { get; private set; }
    }
}