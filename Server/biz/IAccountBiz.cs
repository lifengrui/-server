using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetFrame;


namespace Server.biz
{
    public interface IAccountBiz
    {
        /// <summary>
        /// 帐号创建
        /// </summary>
        /// <param name="token"></param>
        /// <param name="account">注册帐号</param>
        /// <param name="password">注册密码</param>
        /// <returns>返回创建结果 0 成功 1 帐号重复 2 帐号不合法 3 密码不合法</returns>
        int create(UserToken token, string account, string password);

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="token"></param>
        /// <param name="account">帐号</param>
        /// <param name="password">密码</param>
        /// <returns>登录结果 0 成功 -1帐号不存在 -2 帐号在线 -3 密码错误 -4输入不合法</returns>
        int login(UserToken token, string account, string password);

        /// <summary>
        /// 客户端断开连接（下线）
        /// </summary>
        /// <param name="token"></param>
        void close(UserToken token);

        /// <summary>
        /// 获取帐号ID
        /// </summary>
        /// <param name="token"></param>
        /// <returns>返回用户登录的ID</returns>
        int get(UserToken token);

    }
}
