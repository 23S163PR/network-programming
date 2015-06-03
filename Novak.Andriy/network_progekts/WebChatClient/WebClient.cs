using System.Net;
using System.Net.Sockets;
using System.Text;

namespace WebChatClient
{
    public class WebClient
    {
        private const int BuferSize = 256;
        private TcpClient _client;
       

        public WebClient(string host, int port)
        {
            _client = new TcpClient();
            _client.ConnectAsync(IPAddress.Loopback, port);
            
        }

        public void SendMessage(string message)
        {
            var stream = _client.GetStream();
            var messageBytes = Encoding.ASCII.GetBytes(message);
            stream.WriteAsync(messageBytes, 0, messageBytes.Length);
            stream.Flush();
            stream.Dispose();
        }

        //public string GetMessage()
        //{
        //    var buffer = new byte[BuferSize];
           
        //    stream.ReadAsync(buffer, 0, buffer.Length - 1);
        //    stream.Flush();
        //    stream.Close();
        //    return Encoding.ASCII.GetString(buffer);
        //}

        public void WebClientStop()
        {
            _client.Close();    
        }
    }
}
