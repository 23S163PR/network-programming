using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using ChatJsonObject;
using chat_client.CustomMessaqgeControl;

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

            //tbBox.TextChanged += (sender, args) => tbBox.ScrollToEnd();
        }

       
        private void BSend_OnClick(object sender, RoutedEventArgs e)
        {
            if(!tbText.Text.Any())return;
            _client.SendMessage(tbText.Text);
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
                    var formatedData = string.Format("\n>>{0}\n{1}\n{2}", data.Login, data.Message, DateTime.Now);
                    MessageContainer.ApendMessage(data);
                   // tbBox.CheckAppendText(formatedData);
                }
            }
           // if (!_client.ServerAviable) 
                //tbBox.CheckAppendText(string.Format("\n>>{0}\n{1}\n{2}", "Server", "Server not Aviable!!!", DateTime.Now));
        }
    }// MessageContainer.Children.Add(/*new MessageControl(data.Login, data.Message)*/new Label() { Content = data.Message });

    public static class TextBoxExtensions
    {
        public static void CheckAppendText(this TextBoxBase textBox, string msg, bool waitUntilReturn = false)
        {
            Action append = () => textBox.AppendText(msg);
            if (textBox.CheckAccess())
            {
                append();
            }
            else if (waitUntilReturn)
            {
                textBox.Dispatcher.Invoke(append);
            }
            else
            {
                textBox.Dispatcher.BeginInvoke(append);
               
            }
        }
    }

    public static class StackPanelExtensions
    {
        public static void ApendMessage(this StackPanel panel, ChatObject msg, bool waitUntilReturn = false)
        {
            Action append = () => panel.Children.Add(new MessageControl(msg.Login, msg.Message));
            if (panel.CheckAccess())
            {
                append();
            }
            else if (waitUntilReturn)
            {
                panel.Dispatcher.Invoke(append);
            }
            else
            {
                panel.Dispatcher.BeginInvoke(append);

            }
        }
    }
}
