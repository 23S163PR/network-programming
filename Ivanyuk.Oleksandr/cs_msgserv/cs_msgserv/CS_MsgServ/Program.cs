using System;
using System.Net;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

namespace CS_MsgServ
{    
    class Program
    {
        const int BUFF_SIZE = 4096;
        const int Port = 1337;

        static List<Socket> lsClients = new List<Socket>(); //ліст для підключених клієнтів
        static Mutex mt = new Mutex();
        //клієнтський потік
        public static void ClientThread(Object obj)
        {
            Socket sock = (Socket)obj; // анбоксимо обжект в сокет
            mt.WaitOne();
            lsClients.Add(sock); // сейвим конекчений сокет
            Console.WriteLine("[+] {0}", sock.RemoteEndPoint); //виводимо на екран
            mt.ReleaseMutex();            

            int nBytes = 0;
            byte[] buff = new byte[BUFF_SIZE];
            do
            {
                try
                {
                    nBytes = sock.Receive(buff); //розмір данних з відправки
                    TransmitMsg(buff, nBytes, sock);                    
                }
                catch (SocketException ex) // якшо клієнт закрив конект ловимо екзепшн, видаляємо з спску конектів і закриваєм порт
                {
                    mt.WaitOne();
                    lsClients.Remove(sock); // видаляємо активний конект
                    Console.WriteLine("[-] {0}", sock.RemoteEndPoint );
                    mt.ReleaseMutex();
                    sock.Close(); // вбиваємо
                    nBytes = 0;
                }                
            } while (nBytes > 0);
        }
        
        static void TransmitMsg(byte[] buff, int size, Socket fromSock)
        {
            mt.WaitOne();            
            foreach (Socket sock in lsClients)  //сендимо всім крім того сокета з якого пришло
            {
               // if (sock != fromSock)
              //  {
                    sock.Send(buff, size, SocketFlags.None);
              //  }
            }
            mt.ReleaseMutex();
        }

        static void Main(string[] args)
        {
            Thread clThread;                                                    //
            Socket mySocket = new Socket(SocketType.Stream, ProtocolType.Tcp);  //
            IPEndPoint ep = new IPEndPoint(IPAddress.Any, Port);                // створюємо сокет 
            mySocket.Bind(ep);                                                  //
            mySocket.Listen(0);                                                 //
            while (true)
            {
                clThread = new Thread(ClientThread);    // 
                clThread.Start(mySocket.Accept());      // ставимо сокет на прийом зі стартом клієнтського потоку         
            }
        }
    }
}
