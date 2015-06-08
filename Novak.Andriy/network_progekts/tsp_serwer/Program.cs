namespace tsp_serwer
{
    static class Program
    {
        static void Main(string[] args)
        {
            var server = new ChatServer();
            server.AcceptClients();
            server.StopServer();
        }
    }
}
