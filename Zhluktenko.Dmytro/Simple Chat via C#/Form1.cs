using System;
using System.Windows.Forms;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.IO;


namespace WindowsApplication2
{
   
    public partial class Form1 : Form
    {

        public ChatClient obj = new ChatClient();

        public Form1()
        {
            InitializeComponent();
        }

        private void GetMessage()
        {
            while (true) // till window isnt closed
            {
                obj.ServerStream = obj.ClientSocket.GetStream();
                int buffSize = 0;
                byte[] inStream = new byte[obj.BufferSizeBytes];
                buffSize = obj.ClientSocket.ReceiveBufferSize;
                try
                {
                    obj.ServerStream.Read(inStream, 0, buffSize); // read inStream from [0] to [buffSize]
                }
                catch (IOException e)
                {
                    MessageBox.Show(e.Message);
                    Application.Exit();
                    return;
                }
                string returndata = System.Text.Encoding.ASCII.GetString(inStream);
                obj.ReadData = "" + returndata;
                PrintMessage(); // put message to chatbox 
            }

        }

        private void PrintMessage() // set message on chatbox
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(PrintMessage)); // Gets a value indicating whether the caller must call an invoke method 
                                                      //  when making method calls to the control                                     //   because the caller is on a different thread than the one the control was created on.
            }
            else
            {
                ChatBox.Text += Environment.NewLine + " >> " + obj.ReadData; // write data that we got from server in chatBox
            }
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            if (obj.Logined.Equals(true))
            {
                obj.SendMessage(TextMessageBox.Text);
                this.TextMessageBox.Clear();
            }
            else MessageBox.Show("Enter login!");
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.LoginBox.Text) || String.IsNullOrWhiteSpace(this.LoginBox.Text))
            {
                MessageBox.Show("Empty login field!", "Unable to login!");
                return;
            }
            try
            {
                obj.ConnectToServer(this.LoginBox.Text);
                this.Text = this.obj.ServerAddress + " :: " + this.LoginBox.Text;
                Thread ctThread = new Thread(GetMessage); // login and start chat
                ctThread.Start(); 
            }
            catch (SocketException)
            {
                MessageBox.Show("Server is not up!");
                this.Close();
            }      
            this.ConnectButton.Enabled = false;
            this.LoginBox.Enabled = false;
            this.SendButton.Enabled = true;
        
        }

         // create a new window on HOME press 
        // test tool

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Home)
            {
                //MessageBox.Show("", "");
                Form1 f = new Form1();
                f.Show();
            }
        }

        private void LoginBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Home)
            {
                //MessageBox.Show("", "");
                Form1 f = new Form1();
                
                f.Show();
            }
            if (e.KeyCode == Keys.Enter)
            {
                if (!String.IsNullOrEmpty(this.LoginBox.Text))
                {
                    this.obj.ConnectToServer(this.LoginBox.Text);
                    this.LoginBox.Enabled = false;
                }
            }
        }

        private void ChatBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Home)
            {
                //MessageBox.Show("", "");
                Form1 f = new Form1();
                f.Show();
            }
        }

        private void TextMessageBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Home)
            {
                //MessageBox.Show("", "");
                Form1 f = new Form1();
                f.Show();
            }
            else if (e.KeyCode == Keys.Enter) // send message on enter key
            {
                if (!String.IsNullOrEmpty(this.TextMessageBox.Text))
                {
                    this.obj.SendMessage(this.TextMessageBox.Text);
                    this.TextMessageBox.Clear();
                }
            }
        }

    }

}
