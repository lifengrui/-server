using NetFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using NetFrame.auto;



namespace Server
{
    class Program
    {
   

        static void Main(string[] args)
        {
            ServerStart ss = new NetFrame.ServerStart(123);
            ss.encode = MessageEncoding.encode;
            ss.center = new HandlerCenter();
            ss.decode = MessageEncoding.decode;
            ss.LD = LengthEncoding.decode;
            ss.LE = LengthEncoding.encode;

            ss.Start(1234);
            Console.WriteLine("服务器启动成功");
            while (true) { }

        }   
    }
}
