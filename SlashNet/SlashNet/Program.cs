using System;
using Net;

namespace Slash
{
    class MainClass
    {
        public static void Main (string[] args)
        {
			NetSocket net = new NetSocket();
            net.Connect("127.0.0.1", 8881);
            Console.ReadKey();
        }
    }
}
