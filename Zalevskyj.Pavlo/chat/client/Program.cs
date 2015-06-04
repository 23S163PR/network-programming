using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace client
{
    class Program
    {
        static void Main(string[] args)
        {
            var ipHostEntry = Dns.GetHostEntry("");

            const int Port = 4567;

            var client = new TcpClient();
            client.Connect(IPAddress.Loopback, Port);

            var stream = client.GetStream();
          


           while(true)
            {
                var message = Console.ReadLine();
                var messageBytes = Encoding.ASCII.GetBytes(message);
                stream.Write(messageBytes, 0, messageBytes.Length);

            }



            stream.Flush();
            stream.Dispose();
            client.Close();

            Console.WriteLine("Press Enter to exit client ...");
            Console.ReadLine();
        }
    }
}
