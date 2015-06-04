using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    class Client
    {
        private int Port;
        private IPAddress IpAddress;
        private TcpClient client;

        public Client()
        {
            Port = 2335;
            IpAddress = IPAddress.Loopback;
            client = new TcpClient();
        }
        public void SendMessagesToServer(string message)
        {
            client.Connect(IpAddress, Port);
            var stream = client.GetStream();
            var messageBytes = Encoding.ASCII.GetBytes(message);
            Console.WriteLine("Entered message: {0}", message);
            stream.Write(messageBytes, 0, messageBytes.Length);
            stream.Flush();
            client.Close();
        }

    }
}
