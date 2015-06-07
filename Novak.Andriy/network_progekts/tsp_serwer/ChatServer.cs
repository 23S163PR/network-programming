﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Web.Script.Serialization;
using ServerJsonObject;

namespace tsp_serwer
{
    class ChatServer
    {
        private TcpListener _server;
        private List<Socket> _clients;
        private volatile int MaxClientCount = 2;
        private const int ServerPort = 4567;
        private bool _stopNetwork;
        private const int CloseClientCode = 404;
      
        public ChatServer()
        {
            StartServer();
        }

        private void StartServer()
        {
            try
            {
                _clients = new List<Socket>();
                _stopNetwork = false;
                _server = new TcpListener(IPAddress.Any, ServerPort);
                _server.Start();
                var acceptThread = new Thread(AcceptClients);
                acceptThread.Start();
            }
            catch (Exception e) { }
        }

        public void AcceptClients()
        {
            while (!_stopNetwork)
            {
                try
                {
                    
                    var client = _server.AcceptTcpClient();
                    if (_clients.Count > MaxClientCount)
                    {
                        var socket = client.Client;
                        ClientStatus(ref socket, 404/*Disconect client code*/);
                        continue;
                    }
                    _clients.Add(client.Client);
                    var readThread = new Thread(ReceiveRun);
                    readThread.Start(client.Client);
                }
                catch (Exception) { }
            }
        }

        //read add broadcast message
        void ReceiveRun(object client)
        {   
            var message = new StringBuilder();
            var socket = (Socket)client;
            var defaultCode = 1;
            while (socket.Connected)
            {
                if (socket.Available > 0)
                {
                    var buff = new byte[socket.Available];
                   
                    socket.Receive(buff);
                  
                    message.Append(Encoding.ASCII.GetString(buff));
                    defaultCode = message.ToString().JsonToObject().Code;   
                }
                ClientStatus(ref socket, defaultCode);
                if (message.Length > 0 && defaultCode != CloseClientCode)
                {
                    SendToClients(message.ToString());
                    message.Clear();
                }
               Thread.Sleep(100);
            }
        }
       
        private void ClientStatus(ref Socket nClient, int code)
        {
            if (code == 404)
            {
                _clients.Remove(nClient);
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
            if (_server == null)return;
            _server.Stop();
            _server = null;
            _stopNetwork = true;
            foreach (var client in _clients)
            {
                client.Close();
            }  
        }
    }
}
