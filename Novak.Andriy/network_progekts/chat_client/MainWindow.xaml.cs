using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls.Primitives;
using ServerJsonObject;

namespace chat_client
{
    public partial class MainWindow : Window
    {
        private ChatClient _client;
        

        public MainWindow()
        {
            InitializeComponent();
            _client = new ChatClient("user");
          
            Closed += (sender, args) =>
            {
                _client.SendMessage("404", 404/*close client code for server*/);
                
                _client.CloseClient();
            };
        }

       
        private void BSend_OnClick(object sender, RoutedEventArgs e)
        {
            if(!tbText.Text.Any())return;
            _client.SendMessage(tbText.Text);
        }

        private void ConectClick(object sender, RoutedEventArgs e)
        {
            _client.Connect(IPAddress.Loopback, 4567);
            var thread = new Thread(ReciveMessage);
            thread.Start();
        }

        private void ReciveMessage()
        {
            while (true)
            {
                if (!_client.DataAviable) continue;
                var data = _client.ReceiveRun().JsonToObject();
                var formatedData = string.Format("\n>>{0}\n{1}\n{2}", data.Login, data.Message, DateTime.Now);
                tbBox.CheckAppendText(formatedData);
            }
        }
    }

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
}
