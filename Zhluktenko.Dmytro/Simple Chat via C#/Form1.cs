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

        public const int BufferSizeInputBytes = 10025;
        public const int port = 8888;
        public bool logined = false;
        public System.Net.Sockets.TcpClient clientSocket = new System.Net.Sockets.TcpClient();

        public NetworkStream serverStream = default(NetworkStream);

        public string readData = "";


        public Form1()
        {
            InitializeComponent();
        }

        private void getMessage()
        {
            while (true) // till window isnt closed
            {

                serverStream = clientSocket.GetStream();

                int buffSize = 0;

                byte[] inStream = new byte[BufferSizeInputBytes];

                buffSize = clientSocket.ReceiveBufferSize;
                try
                {
                    serverStream.Read(inStream, 0, buffSize); // read inStream from [0] to [buffSize]

                }
                catch (IOException e)
                {
                    MessageBox.Show(e.Message);
                    this.Close();
                }
                string returndata = System.Text.Encoding.ASCII.GetString(inStream);

                readData = "" + returndata;

                msg(); // put message to chatbox 

            }

        }

        private void msg()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(msg)); // Gets a value indicating whether the caller must call an invoke method 
                                                    //  when making method calls to the control 
                                                   //   because the caller is on a different thread than the one the control was created on.
            }
            else
            {
                ChatBox.Text += Environment.NewLine + " >> " + readData; // write data that we got from server in chatBox
            }

        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            if (logined.Equals(true))
            {
                byte[] outStream = System.Text.Encoding.ASCII.GetBytes(TextMessageBox.Text + "$"); // read text to send from TextMessageBox

                serverStream.Write(outStream, 0, outStream.Length);  // write message from [0] to [outStream.Length]

                serverStream.Flush();  // clear all buffers and causes any buffered data to be written to the underlying stream.
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
            this.ConnectButton.Enabled = false;
           
            try
            {
                clientSocket.Connect("127.0.0.1", port); // localhost 
                readData = "Conected to Chat Server ...";
                msg();
                logined = true;
                this.SendButton.Enabled = true;


            }
            catch (SocketException)
            {
                MessageBox.Show("Server is not up!");
                this.Close();
            }
            serverStream = clientSocket.GetStream();



            byte[] outStream = System.Text.Encoding.ASCII.GetBytes(LoginBox.Text + "$");

            serverStream.Write(outStream, 0, outStream.Length); // write username from [0] to [outStream.Length]

            serverStream.Flush(); // clear all buffers 
                                    //causes any buffered data to be written to the underlying stream.

            Thread ctThread = new Thread(getMessage);

            ctThread.Start(); 

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
        }





    }

}
