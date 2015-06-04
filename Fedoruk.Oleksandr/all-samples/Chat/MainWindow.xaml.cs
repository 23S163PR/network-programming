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
        const int Port = 4510;
        //const String Ip = "192.168.56.1";


        public MainWindow()
        {
            InitializeComponent();

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
            var client = new TcpClient();
            // client.Connect(IPAddress.Parse(Ip), Port);
            client.Connect(IPAddress.Loopback, Port);

            var stream = client.GetStream();
            var messageInBytes = Encoding.ASCII.GetBytes(tbMessage.Text);

            stream.Write(messageInBytes, 0, messageInBytes.Length);
            stream.Flush();

            stream.Dispose();
            client.Close();
        }

        public void GetMessages()
        {
            const int MaxMessageSizeInBytes = 1024;

            var buffer = new byte[MaxMessageSizeInBytes];
           // var listener = new TcpListener(IPAddress.Parse(Ip), Port);
            var listener = new TcpListener(IPAddress.Any, Port);
            listener.Start();
            Socket socket;

            while (true)
            {
                try
                {
                    socket = listener.AcceptSocket();
                    var received = socket.Receive(buffer);
                    if (received > 0)
                    {
                        var message = Encoding.ASCII.GetString(buffer, 0, received);

                        tbAllMessages.Dispatcher.Invoke(() => { 
                                                tbAllMessages.Text += message + "\n"; 
                                                tbMessage.Text = "";
                                                tbMessage.Focus();
                        });
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
