using System;
using System.Windows.Controls;

namespace chat_client.CustomMessaqgeControl
{
    public partial class MessageControl : UserControl
    {
        public MessageControl(string login, string message)
        {
            InitializeComponent();
            tbMessage.Text = message;
            lbLogin.Content = login;
            lbDate.Content = DateTime.Now;
        }
    }
}
