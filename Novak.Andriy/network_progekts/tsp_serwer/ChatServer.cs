using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace tsp_serwer
{
    class ChatServer : IDisposable
    {
        private static readonly List<Socket> _clientSockets = new List<Socket>(); 
        private const int MaxClientCount = 2;
        private const int MaxMessageSizeInBytes = 1024;
        private Socket _serverSocket;
        private const int ServerPort = 4567; 
        
        private byte[] bufer = new byte[MaxMessageSizeInBytes];
        
        
        public ChatServer()
        {
            Init();
        }

        private void Init()
        {
            try
            {
                _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                var localEndPoint = new IPEndPoint(IPAddress.Any, ServerPort);
                _serverSocket.Bind(localEndPoint);
                _serverSocket.Listen(MaxClientCount);
            }
            catch (Exception)
            {
                
            }
        }

        public void ServerThread()
        {
            while (true)
            {
                var echoSocket = _serverSocket.Accept();
                if (!_clientSockets.Contains(echoSocket)) _clientSockets.Add(echoSocket);                  
                Console.WriteLine("Connection ");
                Console.WriteLine("From {0} \r\n", echoSocket.RemoteEndPoint);
               
                var thread =  new Thread(EchoTread);
                thread.Start(echoSocket);
            }
        }

        private static void EchoTread(Object EchoFlow)
        {
            // преобразовуємо екземпляр сокета клієнта з Object в Socket
            var EchoSocket = (Socket)EchoFlow;
            // виділяємо буфер для прийняття данних
            var buff = new byte[MaxMessageSizeInBytes];
            while (EchoSocket.Connected)
            {
                // ждемо поки юзер шось не пошле і приймаємо прислані данні в buff
                var received = EchoSocket.Receive(buff);
                // шлем їх йому назад ! )))))))
                if (received > 0)
                {
                    foreach (var client in _clientSockets)
                    {
                        client.Send(buff);
                    } 
                    var receivedMessage = Encoding.UTF8.GetString(buff, 0, received);
                    Console.WriteLine("CLIENT:: {0}", receivedMessage);
                    if (receivedMessage == "404")
                    {
                        _clientSockets.Remove(EchoSocket);
                        EchoSocket.Disconnect(true); 
                    }                  
                }
                
                //}
                // чистимо буффер від того шо отримали, ато заб"ється і буде каша
                Array.Clear(buff, 0, buff.Length);
            }
        }

        public void Dispose()
        {
           _serverSocket.Close();
           _serverSocket.Dispose();
        }
    }
}
