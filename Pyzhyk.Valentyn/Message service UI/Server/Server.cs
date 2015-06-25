using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Server
{
    internal class Server
    {
        private const int MaxMessageSizeInBytes = 1024;
        private readonly List<Client> _clients;
        private readonly int _maxClientCount;
        private readonly int _portRead;
        private readonly int _portWrite;
        private TcpListener _listenerReadPort;
        private TcpListener _listenerWritePort;

        public Server(int maxClientCount)
        {
            _maxClientCount = maxClientCount;
            _clients = new List<Client>(_maxClientCount);
            _portRead = 1338;
            _portWrite = 1337;
            StartListeners();
        }

        /// <summary>
        ///     Initialize and start listeners in the port reading(1338) and writing(1337)
        /// </summary>
        private void StartListeners()
        {
            _listenerReadPort = new TcpListener(IPAddress.Any, _portRead);
            _listenerWritePort = new TcpListener(IPAddress.Any, _portWrite);
            _listenerWritePort.Start();
            _listenerReadPort.Start();
        }

        /// <summary>
        ///     Broadcast messages from all clients
        /// </summary>
        public void ReroutingMessages()
        {
            var socketWrite = _listenerWritePort.AcceptSocket();
            var socketRead = _listenerReadPort.AcceptSocket();

            _clients.Add(new Client(socketRead, socketWrite));
            var thread = new Thread(() =>
            {
                while (socketRead.Connected)
                {
                    var messageBytes = new byte[MaxMessageSizeInBytes];
                    var recieved = socketRead.Receive(messageBytes);
                    Array.Resize(ref messageBytes, recieved);
                    foreach (var client in _clients)
                    {
                        client.SocketWrite.Send(messageBytes);
                    }
                }
                socketRead.Close();
                socketWrite.Close();
                for (var i = 0; i < _clients.Count; i++)
                {
                    if (_clients[i].SocketRead.Equals(socketRead))
                    {
                        _clients.Remove(_clients[i]);
                    }
                }
            });
            thread.IsBackground = false;
            thread.Start();
            ReroutingMessages();
        }
    }
}