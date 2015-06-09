using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsApplication2
{
    public class ChatClient
    {
        public readonly int Port = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["Port"]); // app.config
        public readonly int BufferSizeBytes = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["BufferSizeInputBytes"]); // app.config
        public readonly String ServerAddress = System.Configuration.ConfigurationSettings.AppSettings["ServerAddress"]; // app.config        
        
        public NetworkStream ServerStream;
        public String ReadData;
        public TcpClient ClientSocket;
        public bool Logined;
        
        public ChatClient()
        {
            this.ServerStream = default(NetworkStream);
            this.ClientSocket = new TcpClient();
        }
        public void GetMessage()
        {
            this.ServerStream = this.ClientSocket.GetStream();
            int buffSize = 0;
            byte[] inStream = new byte[this.BufferSizeBytes];
            buffSize = this.ClientSocket.ReceiveBufferSize;
            try
            {
                this.ServerStream.Read(inStream, 0, buffSize);
            }
            catch (IOException e)
            {
                throw new IOException();
            }
            String returndata = System.Text.Encoding.ASCII.GetString(inStream);
            this.ReadData = "" + returndata;


        }
        public void SendMessage(String msgText)
        {
            //try
            //{
                byte[] outStream = System.Text.Encoding.ASCII.GetBytes(msgText + "$"); // read text to send from TextMessageBox
                try
                {
                    this.ServerStream.Write(outStream, 0, outStream.Length);  // write message from [0] to [outStream.Length]
                }
                catch (IOException)
                {
                    MessageBox.Show("Cannot connect to server.");
                    throw new IOException();
                }
                this.ServerStream.Flush();  // clear all buffers and causes any buffered data to be written to the underlying stream.
            //}
        }

        public void ConnectToServer(String login)
        {
            try
            {
                this.ClientSocket.Connect(this.ServerAddress, this.Port); // localhost = 127.0.0.1 , 8888
                this.ReadData = "Conected to Chat Server ...";
                this.Logined = true;
            }
            catch (SocketException)
            {
                throw new SocketException();
            }
            this.ServerStream = this.ClientSocket.GetStream();
            byte[] outStream = System.Text.Encoding.ASCII.GetBytes(login + "$");
            this.ServerStream.Write(outStream, 0, outStream.Length); // write username from [0] to [outStream.Length]
            this.ServerStream.Flush(); // clear all buffers 
                                       //causes any buffered data to be written to the underlying stream.
           
            
            
        }
    }
}
