using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Slash
{
    public class Net
    {
        private Socket clientSocket;
        private ByteBuf buf;


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
            Thread thread = new Thread(new ThreadStart(receive));
            thread.IsBackground = true;
            thread.Start();
        }

        // 异步收包线程.
        private void receive()
        {
            Console.WriteLine("receive");
            int len = this.clientSocket.Receive(buf.GetRaw());
            if (len > 0)
            {
                buf.ReaderIndex(0);
                buf.WriterIndex(len);
                while (true)
                {
                    ByteBuf frame = this.protocal.TranslateFrame(buf);
                    if (frame != null)
                    {
                        this.listner.OnMessage(this, frame);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                this.Close(true);
            }
        }
        
        public static void Main()
        {
            Net net = new Net();
            net.Connect("127.0.0.1", 8881);
            Console.ReadKey();
        }
    }
}