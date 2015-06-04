using System;

using System.Threading;

using System.Net.Sockets;

using System.Text;

using System.Collections;
using System.Net;



namespace ConsoleApplication1
{

    public class ChatServer
    {

        public static Hashtable clientsList = new Hashtable(); // table of clients

        const int port = 8888; // port for server

        static void Main(string[] args)
        {
            IPAddress ipAddress = Dns.Resolve("localhost").AddressList[0]; // get an andress of server 
                                                                           // its gonna be 127.0.0.1
            //IPAddress ipAdress = Dns.GetHostEntry("localhost").AddressList[0];
            TcpListener serverSocket = new TcpListener(ipAddress, port);

            TcpClient clientSocket = default(TcpClient);

            serverSocket.Start();

            Console.WriteLine("Chat Server Started ....");



            while ((true))
            {


                clientSocket = serverSocket.AcceptTcpClient();

                byte[] bytesFrom = new byte[10025];

                string dataFromClient = null;


                NetworkStream networkStream = clientSocket.GetStream();

                networkStream.Read(bytesFrom, 0, (int)clientSocket.ReceiveBufferSize);

                dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);

                dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));



                clientsList.Add(dataFromClient, clientSocket);



                broadcast(dataFromClient + " Joined ", dataFromClient, false);



                Console.WriteLine(dataFromClient + " Joined chat room ");

                handleClinet client = new handleClinet();

                client.startClient(clientSocket, dataFromClient, clientsList);

            }

            clientSocket.Close();

            serverSocket.Stop();

            Console.WriteLine("exit");

            Console.ReadLine();

        }



        public static void broadcast(string msg, string uName, bool flag)
        {

            foreach (DictionaryEntry Item in clientsList) 
            {

                TcpClient broadcastSocket;

                broadcastSocket = (TcpClient)Item.Value;

                NetworkStream broadcastStream = broadcastSocket.GetStream();

                Byte[] broadcastBytes = null;



                if (flag == true)
                {

                    broadcastBytes = Encoding.ASCII.GetBytes(uName + " says : " + msg);

                }

                else
                {

                    broadcastBytes = Encoding.ASCII.GetBytes(msg);

                }



                broadcastStream.Write(broadcastBytes, 0, broadcastBytes.Length);

                broadcastStream.Flush();

            }

        }  //end broadcast function

    }//end Main class





    public class handleClinet
    {
        public const int BufferSizeInputBytes = 10025;

        TcpClient clientSocket;

        string clNo;

        Hashtable clientsList;



        public void startClient(TcpClient inClientSocket, string clineNo, Hashtable cList)
        {

            this.clientSocket = inClientSocket;

            this.clNo = clineNo;

            this.clientsList = cList;

            Thread ctThread = new Thread(doChat);

            ctThread.Start();

        }



        private void doChat()
        {

            int requestCount = 0;

            byte[] bytesFrom = new byte[BufferSizeInputBytes];

            string dataFromClient = null;

            string rCount = null;

            requestCount = 0;



            while ((true))
            {

                try
                {

                    requestCount = requestCount + 1;

                    NetworkStream networkStream = clientSocket.GetStream();

                    networkStream.Read(bytesFrom, 0, (int)clientSocket.ReceiveBufferSize);

                    dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);

                    dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));

                    Console.WriteLine("From client - " + clNo + " : " + dataFromClient);

                    rCount = Convert.ToString(requestCount);



                    ChatServer.broadcast(dataFromClient, clNo, true);

                }

                catch (Exception ex)
                {

                    Console.WriteLine(ex.ToString());

                }

            }//end while

        }//end doChat

    } //end class handleClinet

}//end namespace
