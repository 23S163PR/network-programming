using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows;
using Lib;

namespace Message_service_UI
{
    public class Client
    {
        private const int MaxMessageSizeInBytes = 1024;
        private readonly int _portRead;
        private readonly int _portWrite;
        public IPAddress IpAddress;
        public Socket SocketRead { get; }
        public Socket SocketWrite { get; }
        public string Login { get; set; }
        public bool IsConnected { get; set; }

        /// <summary>
        ///     initialize ports and sockets
        /// </summary>
        public Client()
        {
            _portRead = 1337;
            _portWrite = 1338;
            IpAddress = IPAddress.Loopback;
            Login = "user";
            // connecting to Alex server
            //_portRead = 1337;
            //_portWrite = 1337; 
            //_ipAddress = IPAddress.Parse("192.168.1.50");

            SocketRead = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            SocketWrite = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        ~Client()
        {
            SocketRead.Close();
            SocketWrite.Close();
            SocketWrite.Dispose();
            SocketRead.Dispose();
        }

        /// <summary>
        ///     Connect sockets and starting to get messages
        /// </summary>
        public void StartNetwork(ObservableCollection<Message> messages)
        {
            ConnectSockets();
            GetMessages(messages);
        }

        /// <summary>
        ///     connect sockets
        /// </summary>
        public void ConnectSockets()
        {
            try
            {
                SocketWrite.Connect(IpAddress, _portWrite);
                SocketRead.Connect(IpAddress, _portRead);
                IsConnected = true;
            }
            catch (Exception)
            {
                IsConnected = false;
            }
        }

        /// <summary>
        ///     Send message to server
        /// </summary>
        /// <param name="message"></param>
        public void SendMessageToServer(string message)
        {
            // var messageBytes = Encoding.ASCII.GetBytes(message);
            var messageBytes =
                GlobalMethods.SerializeMessageToBytes(new Message
                {
                    MessageText = message,
                    Login = Login,
                    Time = DateTime.Now
                });

            SocketWrite.Send(messageBytes);
        }

        /// <summary>
        ///     get messages
        /// </summary>
        public void GetMessages(ObservableCollection<Message> messages)
        {
            var thread = new Thread(() =>
            {
                while (SocketRead.Connected)
                {
                    var messageBytes = new byte[MaxMessageSizeInBytes];
                    SocketRead.Receive(messageBytes);

                    // ResizeBuffertoRealSize(ref messageBytes);
                    //var messageString = Encoding.ASCII.GetString(messageBytes, 0, recieved);
                    var message = GlobalMethods.DeserializeBytesToMessage(messageBytes);
                    Application.Current.Dispatcher.Invoke(() => { messages.Add(message); });
                }
            })
            {IsBackground = false};
            thread.Start();
        }
    }
}