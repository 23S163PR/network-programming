using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Windows;
using System.Windows.Input;
using Lib;

namespace Message_service_UI
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Client Client = new Client();

        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
        }

        public ObservableCollection<Message> Messages { get; set; } = new ObservableCollection<Message>();

        public void AddMessage(string message)
        {
            Messages.Add(new Message {MessageText = message, Login = "Login", Time = DateTime.Now});
        }

        private void ImageSendMessage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SendMessage();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            if (Client.IsConnected) return;

            Client.StartNetwork(Messages);
            Client.Login = Login.Text;
            Client.IpAddress = IPAddress.Parse(IP.Text);
            //hide control setting
            //Settings.Width = 0;
        }

        private void TextBoxSendMessage_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;

            SendMessage();
        }

        private void SendMessage()
        {
            Client.SendMessageToServer(MessageTextBox.Text);
            MessageTextBox.Clear();
        }
    }
}