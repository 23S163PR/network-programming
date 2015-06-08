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

    public class Client
    {
        public Socket SocketRead { get; private set; }
        public Socket SocketWrite { get; private set; }

        public Client(Socket socketRead, Socket socketWrite)
        {
            SocketRead = socketRead;
            SocketWrite = socketWrite;
        }

    }
    class Server
    {
        private int PortRead;
        private int PortWrite;
        private int MaxClientCount;
        private const int MaxMessageSizeInBytes = 1024;
        private List<Client> clients;
        private TcpListener ListenerReadPort;
        private TcpListener ListenerWritePort;

        public Server()
        {
            clients = new List<Client>();
            PortRead = 1338;
            PortWrite = 1337;
            StartListeners();
        }
        /// <summary>
        /// Initialize and start listeners in the port reading(1338) and writing(1337)
        /// </summary>
        private void StartListeners()
        {
            ListenerReadPort = new TcpListener(IPAddress.Any, PortRead);
            ListenerWritePort = new TcpListener(IPAddress.Any, PortWrite);
            ListenerWritePort.Start();
            ListenerReadPort.Start();
        }

        public void ReroutingMessages()
        {
            var socketWrite = ListenerWritePort.AcceptSocket();
            var socketRead = ListenerReadPort.AcceptSocket();

            clients.Add(new Client(socketRead, socketWrite));
            Thread thread = new Thread(() =>
            {
                while (socketRead.Connected)
                {
                    byte[] messageBytes = new byte[MaxMessageSizeInBytes];
                    int recieved = 0;

                    try
                    {
                        recieved = socketRead.Receive(messageBytes);
                    }
                    catch(SocketException e)
                    {
                        break;
                    }

                    foreach (var client in clients)
                    {
                        if (client.SocketRead != socketRead)
                        {
                            client.SocketWrite.Send(messageBytes);
                        }
                    }
                    //socketWrite.Send(messageBytes);

                    var messageString = Encoding.ASCII.GetString(messageBytes, 0, recieved);
                    Console.WriteLine(messageString);

                }
                socketRead.Close();
                // socketRead.Dispose();
                socketWrite.Close();
                // socketWrite.Dispose();
                for (int i = 0; i < clients.Count; i++)
                {
                    if (clients[i].SocketRead.Equals(socketRead))
                    {
                        clients.Remove(clients[i]);
                    }
                }


            });
            thread.IsBackground = false;
            thread.Start();
            ReroutingMessages();
        }
    }
}
