using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFrame
{
    class UserTokenPool
    {
        private Stack<UserToken> pool;

        public UserTokenPool(int max)
        {
            pool = new Stack<UserToken>(max);
        }
        //取出一个连接对象
        public UserToken pop()
        {
            return pool.Pop();
        }
        //断开一个连接对象
        public void push(UserToken token)
        {
            if (token != null)
            {
                pool.Push(token);
            }
        }

        public int Size {
            get { return pool.Count; }
        }
    }
}
