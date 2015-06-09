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
       // List<Socket> participants;
        Socket socketRead;
        Socket socketWrite;

        IPEndPoint endPointRead;
        IPEndPoint endPointWrite;
        IPAddress ipAddress; 

        const int PortRead = 3333;
        const int PortWrite = 2222;
        const int MaxParticipants = 2;
        byte[] ipAddressInByte = new byte[] { 192, 168, 56, 1 };

        public Server()
        {
            //participants = new List<Socket>();
       
            ipAddress = new IPAddress(ipAddressInByte);
            endPointRead = new IPEndPoint(ipAddress, PortRead);
            endPointWrite = new IPEndPoint(ipAddress, PortWrite);

            socketRead = new Socket(endPointRead.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socketWrite = new Socket(endPointWrite.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            
        }
        public void Start()
        {
            AcceptMessages();
        }

        private void AcceptMessages()
        {
            socketRead.Bind(endPointRead);
            socketRead.Listen(MaxParticipants);
            while (true)
            {
                try
                {
                    var socketRes = socketRead.Accept();
                   // participants.Add(socketRes);
                    byte[] buffer = new byte[255];
                    var received = socketRes.Receive(buffer);
                    if (received > 0)
                    {    
                        var messageInString = Encoding.ASCII.GetString(buffer, 0, received);
                        Console.WriteLine("New message:  {0}", messageInString);
                        
                        SendMessage(buffer);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("\n\n{0}\n\n", e.Message);
                }
            }
        }

        private void SendMessage(byte[] message)
        {
            socketWrite = new Socket(endPointWrite.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socketWrite.Connect(ipAddress, PortWrite);
            socketWrite.Send(message);
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