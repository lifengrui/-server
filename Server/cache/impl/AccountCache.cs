using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetFrame;
using Server.dao.model;

namespace Server.cache.impl
{


    class AccountCache : IAccountCache
    {
        public int index = 0;
        /// <summary>
        /// 玩家连接对象与账号的映射绑定
        /// </summary>
        Dictionary<UserToken, string> onlineAccMap = new Dictionary<UserToken, string>();
        /// <summary>
        /// 玩家帐号与自身属性的映射绑定
        /// </summary>
        Dictionary<string, AccountModel> accMap = new Dictionary<string, AccountModel>();


        public void add(string account, string password)
        {
            // 连接账号实体并进行绑定
            AccountModel model = new AccountModel();
            model.account = account;
            model.password = password;
            model.id = index;
            accMap.Add(account, model);

            index++;
        }

        public int getId(UserToken token)
        {
            //判断在线字典中有此连接记录，没有说明次连接没有登录，无法获取帐号ID
            if (!onlineAccMap.ContainsKey(token)) return -1;
            return accMap[onlineAccMap[token]].id;
        }

        public bool hasAccount(string account)
        {
            return accMap.ContainsKey(account);
        }

        public bool isOnline(string account)
        {
            //判断当前字典中是否有此帐号 没有说明不在线
            return onlineAccMap.ContainsValue(account);
        }

        public bool match(string account, string password)
        {
            //判断帐号是否存在 
            if (!hasAccount(account)) return false;

            return accMap[account].password.Equals(password);  
        }

        public void offline(UserToken token)
        {
            //如果当前账号有登录，进行移除
            if (onlineAccMap.ContainsKey(token)) onlineAccMap.Remove(token);
        }

        public void online(UserToken token, string account)
        {
            //添加映射
            onlineAccMap.Add(token, account);
        }
    }
}
