using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Message_service_UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Client Client = new Client();
        public MainWindow()
        {
            InitializeComponent();
            Client.ConnectSockets();
            InitializeClient();
        }
        public void InitializeClient()
        {
            //Thread thread = new Thread(() =>
            //{
            //    while (client.SocketWrite.Connected)
            //    {
            //        client.SendMessageToServer(Console.ReadLine());
            //    }
            //});
            //thread.IsBackground = false;
            //thread.Start();

            Client.GetMessages(this);
        }

        public void UIAddMessage(string message, bool isThisClient)
        {
            if (isThisClient)
            {
                // show message right
                MessagesPanel.Dispatcher.Invoke(() =>
                {
                    Grid grid = new Grid() { Margin = new Thickness(0, 5, 20, 5), HorizontalAlignment = HorizontalAlignment.Right };
                    grid.Children.Add(new TextBlock() { Text = message });
                    MessagesPanel.Children.Add(grid);
                });
            }
            else
            {
                // show message left

                MessagesPanel.Dispatcher.Invoke(() =>
                {
                    Grid grid = new Grid() { Margin = new Thickness(20, 5, 0, 5) };
                    grid.Children.Add(new TextBlock() { Text = message });
                    MessagesPanel.Children.Add(grid);
                });
            }

        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Client.SendMessageToServer(MessageTextBox.Text);
            UIAddMessage(MessageTextBox.Text, true);
            MessageTextBox.Clear();
        }

        private void MessageTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Client.SendMessageToServer(MessageTextBox.Text);
                UIAddMessage(MessageTextBox.Text, true);
                MessageTextBox.Clear();
            }
        }
    }


    //public static class StackPanelExtensions
    //{
    //    public static void CheckAppendText(this TextBoxBase textBox, string msg, bool waitUntilReturn = false)
    //    {
    //        Action append = () => textBox.AppendText(msg);
    //        if (textBox.CheckAccess())
    //        {
    //            append();
    //        }
    //        else if (waitUntilReturn)
    //        {
    //            textBox.Dispatcher.Invoke(append);
    //        }
    //        else
    //        {
    //            textBox.Dispatcher.BeginInvoke(append);
    //        }
    //    }
    //}
}
