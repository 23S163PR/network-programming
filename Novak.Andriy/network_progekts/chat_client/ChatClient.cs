using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ServerJsonObject;


namespace chat_client
{
    public class ChatClient 
    {
        private TcpClient _tcpСlient;
        private NetworkStream _networkStream;
        private bool _stopNetwork;
        private readonly StringBuilder _message;
        private string _username;
        public bool DataAviable { get { return _networkStream.DataAvailable; } }

        public ChatClient(string username)
        {
            _username = username;
            _message = new StringBuilder();
        }

        public void Connect(IPAddress adress, int port)
        {
            try
            {
                _tcpСlient = new TcpClient();
                _tcpСlient.Connect(adress, port);
                _networkStream = _tcpСlient.GetStream();
            }
            catch
            {
            }
        }

        public void CloseClient()
        {
            if (_networkStream != null) _networkStream.Close();
            if (_tcpСlient != null) _tcpСlient.Close();
            _stopNetwork = true;
        }

        public void SendMessage(string msg, int code = 1)
        {
           if(_networkStream == null)return;

           var buffer = Encoding.ASCII.GetBytes(new ChatObject(_username, "", code, msg).ObjectToJson());
            _networkStream.WriteAsync(buffer, 0, buffer.Length);  
        }

        public string ReceiveRun()
        {
            _message.Clear();
            if (!_tcpСlient.Connected || _stopNetwork) return null;

            try
            {
                while (_tcpСlient.Available > 0)
                {
                    var buffer = new byte[_tcpСlient.Available]; // create buffer for data
                    _networkStream.ReadAsync(buffer, 0, buffer.Length);
                    _networkStream.FlushAsync();
                    _message.Append(Encoding.ASCII.GetString(buffer));
                }
                return _message.Length <= 0 ? null : _message.ToString();
            }
            catch
            {
                
            }
            return null;
        }       
    }
}
