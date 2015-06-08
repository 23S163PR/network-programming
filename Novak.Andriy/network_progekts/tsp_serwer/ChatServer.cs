using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using ChatJsonObject;

namespace tsp_serwer
{
    class ChatServer
    {
        private TcpListener _server;
        private List<Socket> _clients;
        private volatile int MaxClientCount = 2;
        private const int ServerPort = 4567;
        private bool _stopNetwork;
      
        public ChatServer()
        {
            StartServer();
        }

        private void StartServer()
        {
            _clients = new List<Socket>();
            _stopNetwork = false;
            _server = new TcpListener(IPAddress.Any, ServerPort);
            _server.Start();
            var acceptThread = new Thread(AcceptClients);
            acceptThread.Start();
        }

        public void AcceptClients()
        {
            while (!_stopNetwork)
            {
                try
                {
                    var client = _server.AcceptTcpClient();
                    if (_clients.Count <= MaxClientCount)
                    {
                        Console.WriteLine("{0} conect",client.Client.RemoteEndPoint);
                        _clients.Add(client.Client);
                        var readThread = new Thread(ReceiveRun);
                        readThread.Start(client.Client);
                    }
                    else
                    {
                        var socket = client.Client;
                        ClientStatus(ref socket, ChatCodes.CloseConection);
                    } 
                }
                catch (SocketException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        //read add broadcast message
        void ReceiveRun(object client)
        {   
            var message = new StringBuilder();
            var socket = (Socket)client;
            var defaultCode = ChatCodes.Conected;
            while (socket.Connected)
            {
                if (socket.Available > 0)
                {
                    var buff = new byte[socket.Available];
                   
                    socket.Receive(buff);
                  
                    message.Append(Encoding.ASCII.GetString(buff));
                    var chatObj = message.ToString().JsonToObject();

                    defaultCode = chatObj != null ? chatObj.Code : ChatCodes.Conected; //if not valid json                
                }
                ClientStatus(ref socket, defaultCode);//check if client disconect

                if (message.Length <= 0 || defaultCode == ChatCodes.CloseConection) continue;

                SendToClients(message.ToString());
                message.Clear();
            }
        }

        private void ClientStatus(ref Socket nClient, ChatCodes code)
        {
            if (code == ChatCodes.CloseConection)
            {
                _clients.Remove(nClient);
                Console.WriteLine("{0} disconect",nClient.RemoteEndPoint);
                nClient.Disconnect(false);
            }
        }

        void SendToClients(string text)
        {
            foreach (var client in _clients)
            {
                if (!client.Connected) continue;
                var buffer = Encoding.ASCII.GetBytes(text);
                client.Send(buffer);
            }
        }
 
        public void StopServer()
        {
            foreach (var client in _clients)
            {
                client.Close();
            }  
            if (_server == null)return;
            _server.Stop();
            _server = null;
            _stopNetwork = true;
        }
    }
}
