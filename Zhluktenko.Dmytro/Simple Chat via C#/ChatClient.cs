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
        public readonly int Port = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["Port"]); // port for server
        public readonly int BufferSizeBytes = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["BufferSizeInputBytes"]);
        public NetworkStream ServerStream;
        public String ReadData;
        public TcpClient ClientSocket;
        public bool Logined;
        public readonly String ServerAddress = System.Configuration.ConfigurationSettings.AppSettings["ServerAddress"];
        
        public ChatClient()
        {
            this.ServerStream = default(NetworkStream);
            this.ClientSocket = new TcpClient();
        }

        public void SendMessage(String msgText)
        {
                byte[] outStream = System.Text.Encoding.ASCII.GetBytes(msgText + "$"); // read text to send from TextMessageBox
                this.ServerStream.Write(outStream, 0, outStream.Length);  // write message from [0] to [outStream.Length]
                
                this.ServerStream.Flush();  // clear all buffers and causes any buffered data to be written to the underlying stream.
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
