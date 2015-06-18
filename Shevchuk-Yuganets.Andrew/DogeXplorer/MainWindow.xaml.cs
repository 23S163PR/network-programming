using System.Windows;

namespace DogeXplorer
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
    {
        private DogeTopicsLoader loader = new DogeTopicsLoader();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadMoreTopics_Click(object sender, RoutedEventArgs e)
        {
            var topics = loader.LoadTopics();
			TopicsList.ItemsSource = topics;
		}
    }
}
