using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Chat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Socket socketGet;
        IPEndPoint pointGet;
        IPAddress ipAddr;
        byte[] ipAddressInByte = new byte[] { 192, 168, 56, 1 };

        const int PortSend = 3333;
        const int PortGet = 2222;
        const int MaxMessageSizeInBytes = 255;

        public MainWindow()
        {
            InitializeComponent();
            ipAddr = new IPAddress(ipAddressInByte);

            socketGet = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            pointGet = new IPEndPoint(ipAddr, PortGet);

            var threadGetMessage = new Thread(GetMessages);
            threadGetMessage.IsBackground = true;
            threadGetMessage.Start();
        }

        private void btnSend_Click_1(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(tbMessage.Text))
            {
                SendMessage();
            }
        }

        public void SendMessage()
        {
            var socketSend = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socketSend.Connect(ipAddr, PortSend);
            var messageInBytes = Encoding.ASCII.GetBytes(tbMessage.Text);
            socketSend.Send(messageInBytes);
            tbMessage.Text = "";
            tbMessage.Focus();
        }

        public void GetMessages()
        {     
            socketGet.Bind(pointGet);
            socketGet.Listen(2);
            while (true)
            {
                try
                {
                    var socketRes = socketGet.Accept();
                    byte[] buffer = new byte[MaxMessageSizeInBytes];
                    var received = socketRes.Receive(buffer);
                    if (received > 0)
                    {
                        var message = Encoding.ASCII.GetString(buffer, 0, received);
                        message = message.Remove(message.IndexOf("\0"));
                        tbAllMessages.Dispatcher.Invoke(() => {    tbAllMessages.Text += message + "\n";   });
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        private void Grid_KeyUp_1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !String.IsNullOrEmpty(tbMessage.Text))
            {
                SendMessage();
            }
        }
        

    }
}
