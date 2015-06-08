using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chat_server
{
    public class Server
    {
        List<Socket> participants;
        Socket socketRead;
        Socket socketWrite;

        IPEndPoint endPointRead;
        IPEndPoint endPointWrite;
        IPAddress ipAddress; 

        const int PortRead = 3333;
        const int PortWrite = 2222;
        const int MaxParticipants = 2;

        public Server()
        {
            participants = new List<Socket>();

            byte[] ipAddressInByte = new byte[] { 192, 168, 56, 1 };
            ipAddress = new IPAddress(ipAddressInByte);

            endPointRead = new IPEndPoint(ipAddress, PortRead);
            endPointWrite = new IPEndPoint(ipAddress, PortWrite);


            socketRead = new Socket(endPointRead.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socketWrite = new Socket(endPointWrite.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            
        }
        public void Start()
        {
            //new Thread(AcceptMessages).Start();
            AcceptMessages();
        }

        private void AcceptMessages()
        {
            socketRead.Bind(endPointRead);
            socketRead.Listen(MaxParticipants);
            while (true)
            {
                Console.WriteLine("Waiting for a connection...");
                var socket = socketRead.Accept();
                participants.Add(socket);
                byte[] bArr = new byte[255];
                var messageInByte = socket.Receive(bArr);
                var messageInString = Encoding.ASCII.GetString(bArr, 0, messageInByte);

                Console.WriteLine(messageInString);
                socket.Send(bArr);
                
            }


        }

    }

    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server();
            server.Start();
        }

    }
}