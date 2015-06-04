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
        private int PortRead;
        private int PortWrite;
        private IPAddress IpAddress;
        private const int MaxMessageSizeInBytes = 1024;

        private Socket SocketRead;
        private Socket SocketWrite;

        public Client()
        {
            PortRead = 1337;
            PortWrite = 1338;
            //IpAddress = IPAddress.Parse("192.168.1.99");
            IpAddress = IPAddress.Loopback;
            SocketRead = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            SocketWrite = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ConnectSockets();
        }
        /// <summary>
        /// connect sockets
        /// </summary>
        private void ConnectSockets()
        {
            SocketRead.Connect(IpAddress, PortRead);
            SocketWrite.Connect(IpAddress, PortWrite);
        }

        public void SendMessageToServer(string message)
        {
            byte[] messageBytes= new byte[message.Length];
            messageBytes = Encoding.ASCII.GetBytes(message);
            SocketWrite.Send(messageBytes);
        }

        public void GetMessages()
        {
            Thread thread = new Thread(() =>
            {
                while (true)
                {
                    byte[] messageBytes = new byte[MaxMessageSizeInBytes];
                    SocketRead.Receive(messageBytes);
                    ResizeBuffertoRealSize(ref messageBytes);
                    var messageString = Encoding.ASCII.GetString(messageBytes);

                    Console.WriteLine(messageString);
                }
            });
            thread.IsBackground = false;
            thread.Start();

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
