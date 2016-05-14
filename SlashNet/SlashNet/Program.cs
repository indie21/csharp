using System;

namespace Slash
{
    class MainClass
    {
        public static void Main (string[] args)
        {
            Net net = new Net();
            net.Connect("127.0.0.1", 8881);
            Console.ReadKey();
        }
    }
}
