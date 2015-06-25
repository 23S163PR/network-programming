using System;

namespace Server
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Server");
            var Server = new Server(5);
            Server.ReroutingMessages();
        }
    }
}