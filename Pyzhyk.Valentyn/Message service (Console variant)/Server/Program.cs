using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Server");
            var Server = new Server();
            Server.StartListenAllIPAdressInPort(1337, 2);
        }
    }
}
