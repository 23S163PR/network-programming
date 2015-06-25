using System.Windows.Controls;
using System.Windows.Input;

namespace Message_service_UI
{
    /// <summary>
    ///     Interaction logic for MessageControl.xaml
    /// </summary>
    public partial class MessageControl : UserControl
    {
        public MessageControl()
        {
            InitializeComponent();
        }

        private void TextBlock_MouseLeave(object sender, MouseEventArgs e)
        {
            Login.Opacity = 0.4;
        }

        private void TextBlock_MouseEnter(object sender, MouseEventArgs e)
        {
            Login.Opacity = 1.0;
        }
    }
}