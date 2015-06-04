using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Echo_1
{
    class Server
    {
        // екземпляр сокета сервера
        Socket ServerSocket;
        // порт сервера
        int Port;
        /// <summary>
        /// конструюємо сервер сокет
        /// </summary>
        public Server()
        {
            ServerSocket =  new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Port = Program.PORT;
            // створюєм ендпоінт, біндим до нього серверний сокет і слухаєм
            EndPoint localEndPoint = new IPEndPoint(IPAddress.Any, Port);
            ServerSocket.Bind(localEndPoint);
            ServerSocket.Listen(10);
        }
        /// <summary>
        /// функция чекає конекта клієнта і вертає екземпляр сокета асоциірованого з ним
        /// </summary>
        /// <returns>ServerSocket.Accept()</returns>
        public Socket Accept()
        {
            return ServerSocket.Accept();
        }
    }
}
