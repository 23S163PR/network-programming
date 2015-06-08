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

        public Socket SocketRead { get; private set; }
        public Socket SocketWrite { get; private set; }

        /// <summary>
        /// initialize ports and sockets
        /// </summary>
        public Client()
        {
            PortRead = 1337;
            PortWrite = 1337;
            IpAddress = IPAddress.Parse("192.168.1.99");
            //IpAddress = IPAddress.Loopback;

            SocketRead = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            SocketWrite = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        }

        /// <summary>
        /// connect sockets
        /// </summary>
        public void ConnectSockets()
        {
            SocketRead.Connect(IpAddress, PortRead);
            SocketWrite.Connect(IpAddress, PortWrite);
        }

        /// <summary>
        /// Send message to server
        /// </summary>
        /// <param name="message"></param>
        public void SendMessageToServer(string message)
        {
            byte[] messageBytes = new byte[message.Length];
            messageBytes = Encoding.ASCII.GetBytes(message);
            SocketWrite.Send(messageBytes);
            //SocketWrite.Close();
            //SocketRead.Close();
            //testing
            //SocketRead.Disconnect(false);
            //SocketWrite.Disconnect(false);
            //testing
        }

        /// <summary>
        /// get messages
        /// </summary>
        public void GetMessages()
        {
            Thread thread = new Thread(() =>
            {
                while (SocketRead.Connected)
                {
                    byte[] messageBytes = new byte[MaxMessageSizeInBytes];
                        var rcv = SocketRead.Receive(messageBytes);

                    ResizeBuffertoRealSize(ref messageBytes, rcv);
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
        private void ResizeBuffertoRealSize(ref byte[] arrayToResize, int rcv)
        {
            Array.Resize(ref arrayToResize, rcv);
        }

    }
}
