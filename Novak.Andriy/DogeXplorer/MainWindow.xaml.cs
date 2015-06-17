using System.Windows;

namespace DogeXplorer
{
    public partial class MainWindow : Window
    {
        private readonly DogeTopicsLoader _loader = new DogeTopicsLoader();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadMoreTopics_Click(object sender, RoutedEventArgs e)
        {
            var topics = _loader.LoadTopics();
            TopicsList.ItemsSource = topics;
        }
    }
}
