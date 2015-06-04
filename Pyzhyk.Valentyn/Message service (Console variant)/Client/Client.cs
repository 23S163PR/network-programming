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
            Port = 1337;
            //IpAddress = IPAddress.Parse("192.168.1.99");
            IpAddress = IPAddress.Loopback;
            client = new TcpClient();
        }

        public void SendMessagesToServer(string message)
        {
            client.Connect(IpAddress, Port);
            var stream = client.GetStream();
            var a = client.Client;
            while (true)
            {
                byte[] bytesread = new byte[1024];
                var messageBytes = Encoding.ASCII.GetBytes(Console.ReadLine());
                //Console.WriteLine("Entered message: {0}", message);
                stream.Write(messageBytes, 0, messageBytes.Length);
                stream.Flush();

                stream.Read(bytesread, 0, bytesread.Length);
                ResizeBuffertoRealSize(ref bytesread);

                Console.WriteLine(Encoding.ASCII.GetString(bytesread, 0, bytesread.Length));
            }
            client.Close();
        }


        /// <summary>
        /// Minimize array
        /// </summary>
        /// <param name="arrayToResize"></param>
        private void ResizeBuffertoRealSize (ref byte[] arrayToResize)
        {
            for (int i = arrayToResize.Length - 1; i > 0; i--)
                if (arrayToResize[i] == 0)
                    Array.Resize(ref arrayToResize, arrayToResize.Length - 1);

        }

    }
}
