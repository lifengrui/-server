using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetFrame;
using NetFrame.auto;
using GameProtocol;
using GameProtocol.dto;
using Server.tool;
using Server.biz;
  

namespace Server.logic.login
{
    public class LoginHandler : AbsOnceHandler, HandlerInterface
    {
        IAccountBiz accountBiz = BizFactory.accountBiz;

        public void ClientClose(UserToken token, string error)
        {
            ExecutorPool.Instacne.execute(
                 delegate ()
                 {
                     accountBiz.close(token);
                 }
                 );


        }

        public void ClientConnent(UserToken token)
        {


        }

        public void MessageReceive(NetFrame.UserToken token, NetFrame.auto.SocketModel message)
        {
            switch (message.command)
            {
                case LoginProtocol.LOGIN_CREQ:
                    login(token, message.GetMessage<AccountInfoDTO>());
                    break;
                case LoginProtocol.REG_CREQ:
                    reg(token, message.GetMessage<AccountInfoDTO>());
                    break;

            }     
            
        }

        public void login(UserToken token, AccountInfoDTO value)
        {
            ExecutorPool.Instacne.execute(
                 delegate ()
                 {
                     int result = accountBiz.login(token, value.account, value.password);
                     write(token, LoginProtocol.LOGIN_SRES, result);
                 }
                 );



        }

        public void reg(UserToken token, AccountInfoDTO value)
        {
            ExecutorPool.Instacne.execute(
               delegate ()
               {
                   int result = accountBiz.create(token, value.account, value.password);
                   write(token, LoginProtocol.REG_SRES, result);
               }
               );
        }


        public override byte GetType()
        {
            return Protocol.TYPE_LOGIN;
        }
    }
    
    
}
