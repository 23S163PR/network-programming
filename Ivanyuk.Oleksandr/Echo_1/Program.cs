using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Echo_1
{
    class Program
    {
        public static int PORT = 1337;
        static void Main(string[] args)
        {
            // створюємо і запускаємо серверний потік
            Thread ServerThread = new Thread(new ThreadStart(Threads.ServerThread));
            ServerThread.Start();
        }
    }
}
