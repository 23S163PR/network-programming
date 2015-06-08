using System.Net;
using System.Net.Sockets;
using System.Text;
using ChatJsonObject;

namespace chat_client
{
    public class ChatClient 
    {
        private TcpClient _tcpСlient;
        private readonly StringBuilder _message;
        public string Username { get; set; }
        public bool ServerAviable { get; private set; }

        public bool StopNetwork { get; private set; }

        public ChatClient()
        {
            _message = new StringBuilder();
        }

        
        public void Connect(IPAddress adress, int port)
        {
            if (_tcpСlient != null)
            {
                 SendMessage("404", ChatCodes.CloseConection);
            }
            _tcpСlient = new TcpClient();
            _tcpСlient.Connect(adress, port); // can return SocketException
            StopNetwork = false;
            ServerAviable = true;
        }

        public void CloseClient()
        {
            if (_tcpСlient != null) _tcpСlient.Close();

            StopNetwork = true;
        }

        public void SendMessage(string msg, ChatCodes code = ChatCodes.Conected)
        {
            if (StopNetwork) return;

            var buffer = Encoding.ASCII.GetBytes(new ChatObject(Username, "", code, msg).ObjectToJson());
            _tcpСlient.Client.Send(buffer);
        }

        public ChatObject ReceiveRun()
        {
            _message.Clear(); 

            if (StopNetwork) return null;

            try
            {
                var buffer = new byte[_tcpСlient.Client.Available]; // create buffer for data
                _tcpСlient.Client.Receive(buffer);
                _message.Append(Encoding.ASCII.GetString(buffer));
                var json = _message.ToString().JsonToObject();
               
                if (json != null) IsServerAviable(json.Code);
                
                return json ?? new ChatObject("Anonimus","", ChatCodes.Conected, _message.ToString());
            }
            catch (SocketException e)
            {
                CloseClient();
            }
            return null;
        }

        private void IsServerAviable(ChatCodes code)
        {
            if (code == ChatCodes.StopServer)
            {
                CloseClient();
                ServerAviable = false;
            }
        }
    }
}
