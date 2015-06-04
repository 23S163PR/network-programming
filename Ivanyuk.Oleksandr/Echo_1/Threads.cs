using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Echo_1
{
    class Threads
    {
        // потік обробки клієнта, приймає екземпляр сокета клієнта
        public static void EchoTread(Object EchoFlow)
        {
            // преобразовуємо екземпляр сокета клієнта з Object в Socket
            Socket EchoSocket = (Socket)EchoFlow;
            // виділяємо буфер для прийняття данних
            byte[] buff = new byte[1024];
            // шлемо клієнту месагу Echo server
            EchoSocket.Send(Transfer.StringToBytes("Echo server\r\n"));
            // поки юзер не закриє коннект працює цей код
            while (EchoSocket.Connected)
            {
                // ждемо поки юзер шось не пошле і приймаємо прислані данні в buff
                EchoSocket.Receive(buff);
                // шлем їх йому назад ! )))))))
                EchoSocket.Send(buff);
                // чистимо буффер від того шо отримали, ато заб"ється і буде каша
                Array.Clear(buff, 0, buff.Length);
            }
        }

        public static void ServerThread() // серверний потік
        {
            // створюємо екземпляр класса server
            Server server = new Server();
            // сокет для приконекчених юзерів
            Socket EchoSocket;
            Thread EchoTread;

            while (true)
            {
                EchoSocket = server.Accept(); // ждем конекта
                Console.WriteLine("Connection ");
                Console.WriteLine("From {0} \r\n", EchoSocket.RemoteEndPoint);
                // на кожного нового юзера створюємо новий потік для обробки данних
                EchoTread = new Thread(Threads.EchoTread);
                EchoTread.Start(EchoSocket); // старт приймає тільки обжект
            }
        }

    }
}
