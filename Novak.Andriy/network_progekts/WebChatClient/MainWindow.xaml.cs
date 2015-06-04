using System.Net;
using System.Windows;
using System.Windows.Documents;

namespace WebChatClient
{
    public partial class MainWindow : Window
    {
        private readonly WebClient _webClient;
        public MainWindow()
        {
            InitializeComponent();
            _webClient = new WebClient();
            Closed += (sender, args) =>
            {
                _webClient.SendMessage("404");
                WebClient.WebClientStop();
            };
        }

        private void BSend_OnClick(object sender, RoutedEventArgs e)
        {
            _webClient.SendMessage(tbText.Text);
            Application.Current.Dispatcher.Invoke(() => { tbBox.Text += _webClient.GetMessage(); });
           
               
        }

        private void ConectClick(object sender, RoutedEventArgs e)
        {
           if(_webClient.ConectToSerwer(IPAddress.Loopback, 4567))
            {
                Title = "I`m conect";
            }
                   
        }
    }
}
