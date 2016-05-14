using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Slash2;

namespace Slash
{
    public class Test
    {
        static public void Main()
        {
            async();
            Console.ReadKey();
        }

        // 同步连接服务器
        static public void sync()
        {
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            Socket clientSocket = new Socket(AddressFamily.InterNetwork,
                                             SocketType.Stream,
                                             ProtocolType.Tcp);
            try
            {
                clientSocket.Connect(new IPEndPoint(ip, 8881)); //配置服务器IP与端口
                Console.WriteLine("连接服务器成功");
            }
            catch
            {
                Console.WriteLine("连接服务器失败，请按回车键退出！");
            }
        }
    }
}