using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetFrame;
using Server.logic;
using Server.logic.login;
using NetFrame.auto;
using GameProtocol;

namespace Server
{
    public class HandlerCenter : AbsHandlerCenter
    {
        HandlerInterface login;

        public  HandlerCenter()
        {
            login = new LoginHandler();
        }

        public override void ClientClose(UserToken token, string error)
        {
            Console.WriteLine("有客户端断开连接了");
            login.ClientClose(token, error);

        }

        public override void ClientConnent(UserToken token)
        {
            Console.WriteLine("有客户端连接了");
        }

        public override void MessageReceive(UserToken token, object message)
        {
            Console.WriteLine("有客户发送消息了");
            SocketModel model = message as SocketModel;
            switch (model.type)
            {
                case Protocol.TYPE_LOGIN:
                    login.MessageReceive(token, model);
                    break;
             
            }
        }
    }
}
