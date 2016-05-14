using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;

namespace Net
{
    public class NetSocket
    {

        const int BUFF_SIZE =  65535;
        const int HEAD_SIZE =  4;
        private Socket clientSocket;
        private ByteBuf buf;

		public NetSocket()
        {
            buf = new ByteBuf(BUFF_SIZE);
        }

        public void Connect(string ip, int port)
        {
            this.clientSocket = new Socket(AddressFamily.InterNetwork,
                                           SocketType.Stream,
                                           ProtocolType.Tcp);

            this.clientSocket.NoDelay = true;
            LingerOption linger = new LingerOption(false, 0);
            this.clientSocket.LingerState = linger;
            this.clientSocket.BeginConnect(ip, port, connected_cb, this);
        }

        private void connected_cb(IAsyncResult ar)
        {
            if (!this.clientSocket.Connected)
            {
                return;
            }

            this.clientSocket.EndConnect(ar);
            Thread thread = new Thread(new ThreadStart(waitSocket));
            thread.IsBackground = true;
            thread.Start();
        }

        public void Close()
        {
            if (clientSocket != null && clientSocket.Connected)
            {
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
            }
        }

        // 异步收包线程.
        private void waitSocket()
        {
            Console.WriteLine("receiver {0}", 1);
            int len;

            while(true)
            {
                if (!this.clientSocket.Poll(-1, SelectMode.SelectRead))
                {
                    Console.WriteLine("poll error");
                    this.Close();
                }

                len = clientSocket.Receive(buf.GetRaw(), 0, HEAD_SIZE,
                                           SocketFlags.None);

                Console.WriteLine("receive length 1 {0}", len);

                if(len<0)
                {
                    Console.WriteLine("length error");
                    this.Close();
                    return;
                }

                Debug.Assert(len == HEAD_SIZE);
                
                int payload_length = buf.GetInt(0);
                
                Debug.Assert(payload_length < BUFF_SIZE);
 
                len = clientSocket.Receive(buf.GetRaw(),0, payload_length,
                                           SocketFlags.None);

                Console.WriteLine("receive length 2 {0}", len);

                Debug.Assert(payload_length == len);
                string data = BitConverter.ToString(buf.GetRaw(), 0, payload_length);
                Console.WriteLine("receiver: {0}", data);
            }
        }            
    }
}