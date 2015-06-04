using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace WebChatClient
{
    public class WebClient
    {
        private const int BuferSize = 1024;
        private static TcpClient _client = new TcpClient {SendBufferSize = BuferSize};

        public bool ConectToSerwer(IPAddress host, int port)
        {
            try
            {
                _client.ConnectAsync(host, port);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public void SendMessage(string message)
        {
            if(!_client.Connected)return;
            var stream = _client.GetStream();
            var messageBytes = Encoding.UTF8.GetBytes(message);
            stream.WriteAsync(messageBytes, 0, messageBytes.Length);
            stream.FlushAsync();
        }

        public string GetMessage()
        {
            var buffer = new byte[BuferSize];
            var stream = _client.GetStream();
            var bytes = stream.ReadAsync(buffer, 0, buffer.Length - 1).Result;
            stream.FlushAsync();
            return Encoding.UTF8.GetString(buffer, 0, bytes);
        }

        public static void WebClientStop()
        {
            _client.Close();    
        }
    }
}
