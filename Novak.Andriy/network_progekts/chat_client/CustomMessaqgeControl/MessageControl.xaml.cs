using System;
using System.Windows;
using System.Windows.Controls;

namespace chat_client.CustomMessaqgeControl
{
    public partial class MessageControl : UserControl
    {
        public MessageControl(string login, string message, bool aligmentFlag)
        {
            InitializeComponent();
            HorizontalAlignment = aligmentFlag ? HorizontalAlignment.Left : HorizontalAlignment.Right;
            tbMessage.Text = message;
            lbLogin.Content = login;
            lbDate.Content = DateTime.Now;
        }
    }
}
