using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class Server
    {
        private int Port;
        private int MaxClientCount;
        private const int MaxMessageSizeInBytes = 1024;

        public void StartListenAllIPAdressInPort(int port, int maxClientCount)
        {
            Port = port;
            MaxClientCount = maxClientCount;
            var listener = new TcpListener(IPAddress.Any, Port);
            listener.Start(MaxClientCount);

            var buffer = new byte[MaxMessageSizeInBytes];
            var socket = listener.AcceptSocket();
            while (true)
            {   

                var received = socket.Receive(buffer);

                if (received > 0)
                {
                    var receivedMessage = Encoding.ASCII.GetString(buffer, 0, received);
                    Console.WriteLine("CLIENT:: {0}", receivedMessage);
                }
            }
        }
    }
}
