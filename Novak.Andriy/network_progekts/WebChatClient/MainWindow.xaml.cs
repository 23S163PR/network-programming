using System.Net;
using System.Windows;

namespace WebChatClient
{
    public partial class MainWindow : Window
    {
        private readonly WebClient _webClient;
        public MainWindow()
        {
            InitializeComponent();
            _webClient = new WebClient(IPAddress.Loopback.ToString(),4567);
        }

        private void BSend_OnClick(object sender, RoutedEventArgs e)
        {
            _webClient.SendMessage(tbText.Text);

        }
    }
}
