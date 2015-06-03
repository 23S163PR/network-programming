using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace tcp_listener
{
    class Program
    {
        static void Main(string[] args)
        {
            var ipHostEntry = Dns.GetHostEntry("");

            const int Port = 4567;
            const int MaxClientCount = 5;
            const int MaxMessageSizeInBytes = 1024;
            
            var listener = new TcpListener(IPAddress.Loopback, Port);
            listener.Start(MaxClientCount);

            var buffer = new byte[MaxMessageSizeInBytes];
            var socket = listener.AcceptSocket();
            var received = socket.Receive(buffer);

            if (received > 0)
            {
                var receivedMessage = Encoding.ASCII.GetString(buffer, 0, received);
                Console.WriteLine("CLIENT:: {0}", receivedMessage);
            }

            socket.Close();
            listener.Stop();

            Console.WriteLine("Press Enter to exit server ...");
            Console.ReadLine();
        }
    }
}
