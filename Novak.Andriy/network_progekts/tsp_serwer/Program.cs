﻿using System.Threading;

namespace tsp_serwer
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new ChatServer();
            server.AcceptClients();
            server.StopServer();
        }
    }
}
