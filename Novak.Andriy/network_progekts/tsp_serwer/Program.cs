using System.Threading;

namespace tsp_serwer
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new ChatServer();
            var serverthread = new Thread(server.ServerThread);
            serverthread.Start();
            Thread.CurrentThread.Join();
            //Console.ReadLine();
        }
    }
}
