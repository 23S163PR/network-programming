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
            var Server = new Server();
            Server.StartListenAllIPAdressInPort(2335, 100);

            Console.ReadLine();


            Console.ReadLine();

        }
    }
}
