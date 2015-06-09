using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using chat_client.CustomMessaqgeControl;
using ChatJsonObject;

namespace chat_client
{
    public partial class MainWindow : Window
    {
        private readonly ChatClient _client;
        

        public MainWindow()
        {
            InitializeComponent();
            tbIp.Text = IPAddress.Loopback.ToString();
            tbPort.Text = "4567";
            _client = new ChatClient();
          
            Closed += (sender, args) =>
            {
                _client.SendMessage("404", ChatCodes.CloseConection);
                
                _client.CloseClient();
            };
        }

       
        private void BSend_OnClick(object sender, RoutedEventArgs e)
        {
            if(!tbText.Text.Any() || _client.StopNetwork)return;

            _client.SendMessage(tbText.Text);
            ContentViewer.ScrollToEnd();
        }

        private void ConectClick(object sender, RoutedEventArgs e)
        {
            try
            {
                IPAddress ip;
                int port;

                if (!IPAddress.TryParse(tbIp.Text, out ip) || !int.TryParse(tbPort.Text, out port))
                {
                    MessageBox.Show("Incorect Ip or Port");
                    return;
                }

                if (!tbLogin.Text.Any())
                {
                    MessageBox.Show("Enter Login");
                    return;
                }

                _client.Username = tbLogin.Text;
                _client.Connect(IPAddress.Parse(tbIp.Text), int.Parse(tbPort.Text));
            }
            catch (SocketException socketException)
            {
                MessageBox.Show(socketException.Message);
                return;
            }
            var thread = new Thread(ReciveMessage);
            thread.Start();
        }

        private void ReciveMessage()
        {
            while (!_client.StopNetwork)
            {
                var data = _client.ReceiveRun();
                if (data != null)
                {
                    if (data.Message.Length <= 0) continue;                  
                    MessageContainer.ApendMessage(data, _client.Username == data.Login); 
                }
            }
        }
    }

    public static class StackPanelExtensions
    {
        public static void ApendMessage(this StackPanel panel, ChatObject msg, bool aligmentFlag)
        {
            Action append = () => panel.Children.Add(new MessageControl(msg.Login, msg.Message,aligmentFlag)); 
            panel.Dispatcher.BeginInvoke(append);   
        }
    }
}
