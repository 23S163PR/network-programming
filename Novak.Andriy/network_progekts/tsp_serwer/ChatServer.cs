using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace tsp_serwer
{
    class ChatServer : IDisposable
    {
        private List<TcpClient> _clients; 
        private Timer _timer;
        
        private const int MaxClientCount = 2;
        private const int MaxMessageSizeInBytes = 1024;
        private Encoding m_encoding = Encoding.ASCII;
        private readonly TcpListener _serverListener;
        private byte[] bufer = new byte[MaxMessageSizeInBytes];
        public static int ServerPort { get { return 4567; }}
        
        public ChatServer()
        {
            _serverListener = new TcpListener(IPAddress.Any, ServerPort);
            _serverListener.Start(MaxClientCount);
            _clients = new List<TcpClient>();
            _timer = new Timer(GetMessageCalback,false, 0, 10000);
        }

        private void GetMessageCalback(object e)
        {
            GetMessage();
            //var newClient = _serverListener.AcceptTcpClientAsync().Result;
            //if(!_clients.Contains(newClient)) _clients.Add(newClient);
            //var res = GetMessage();
            //if (res != null)
            //{
            //    ServerSendingMessages(res);
            //}
        }

        private string GetMessage()
        {
            //var newClient = _serverListener.AcceptTcpClientAsync().Result;
            //if (!_clients.Contains(newClient)) _clients.Add(newClient);
            var socket = _serverListener.AcceptSocketAsync().Result;
            if (socket == null) return null;
            var received = socket.Receive(bufer);
            if (received > 0)
            {
                var receivedMessage = m_encoding.GetString(bufer, 0, received);
                Console.WriteLine(receivedMessage);
                return receivedMessage;
            }
            
            socket.Close();
            return null;
        }

        public void ServerSendingMessages(string message)
        {
            Parallel.ForEach(_clients, c =>
            {
                
            });
            //var buffer = new byte[MaxMessageSizeInBytes];
            //var socket = listener.AcceptSocket();
            //var received = socket.Receive(buffer);

            //if (received > 0)
            //{
            //    var receivedMessage = Encoding.ASCII.GetString(buffer, 0, received);
            //    Console.WriteLine("CLIENT:: {0}", receivedMessage);
            //}

            //socket.Close();
            //listener.Stop();

            //Console.WriteLine("Press Enter to exit server ...");
            //Console.ReadLine();
        }

        public void Dispose()
        {
            _timer.Dispose();
            _serverListener.Stop();

        }
    }
}
