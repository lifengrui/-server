using Server.biz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetFrame;
using Server.cache;

namespace Server.biz.impl
{
    public class AccountBiz : IAccountBiz
    {
        IAccountCache accountCache = CacheFactory.accountCache;

        public void close(UserToken token)
        {
            
        }

        public int create(UserToken token, string account, string password)
        {
            if (accountCache.hasAccount(account)) return 1;
            //待完善
            accountCache.add(account, password);

            return 0;
            
        }

        public int get(UserToken token)
        {  
            return accountCache.getId(token);
        }

        public int login(UserToken token, string account, string password)
        {   
            //判断输入输入是否合法
            if (account == null || password == null) return -4;
            //判断帐号是否存在
            if (!accountCache.hasAccount(account)) return -1;
            //判断帐号是否在线
            if (accountCache.isOnline(account)) return -2;
            //判断密码是否匹配
            if (!accountCache.match(account, password)) return -3;
            //通过验证 说明可以的登录 调用上线 返回成功
            accountCache.online(token, account);
            return 0;
        }
    }
}
