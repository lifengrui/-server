using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
using System.Net;

namespace NetFrame
{
    public class ServerStart
    {
        Socket server;//socket监听对象
        int maxClient;//客户端最大连接数
        Semaphore acceptClients;
        UserTokenPool pool;


        public LengthEncode LE;
        public LengthDecode LD;
        public encode encode;
        public decode decode;
        /// <summary>
        /// 消息处理中心，由外部传入
        /// </summary>
        public AbsHandlerCenter center;

        //初始化监听

     

        public  ServerStart(int max) {
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //设定服务器最大连接人数
            maxClient = max;
    

        }

        public void Start(int port)
        {
            //创建连接池
            pool = new UserTokenPool(maxClient);
            //连接信号量
            acceptClients = new Semaphore(maxClient, maxClient);

            for (int i = 0; i < maxClient; i++)
            {
                UserToken token = new UserToken();
                //初始化token信息
                token.sendSAEA.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Comleted);

                token.receiveSAEA.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Comleted);

                token.LD = LD;
                token.LE = LE;
                token.encode = encode;
                token.decode = decode;
                token.sendProcess = ProcessSend;
                token.closeProcess = ClientClose;
                token.center = center;

                pool.push(token);

            }

            //通过port端口监听当前所有ip
            try
            {
                server.Bind(new IPEndPoint(IPAddress.Any, port));
                //置于监听状态
                server.Listen(10);
                StartAccept(null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void StartAccept(SocketAsyncEventArgs e)
        {
            if (e == null)
            {
                e = new SocketAsyncEventArgs();
                e.Completed += new EventHandler<SocketAsyncEventArgs>(Accept_Comleted );
            }
            else
            {
                e.AcceptSocket = null;
            }
            //信号量-1
            acceptClients.WaitOne();
            bool result = server.AcceptAsync(e);
            //判断异步事件是否挂起，没挂起说明立刻执行完成 直接处理事件 否者会在处理完成后出发Accept_Comleted
            if (!result)
            {
                ProcessAccept(e);
            }
        }

        public void ProcessAccept(SocketAsyncEventArgs e) {
            UserToken token = pool.pop();
            
            token.conn = e.AcceptSocket;
            //TO Do 通知应用层有客户连接
            center.ClientConnent(token);
            //开启消息到达监听
            StartReceive(token);

            //释放当前异步对象
            StartAccept(e);
        }
         
        public void Accept_Comleted(object sender,SocketAsyncEventArgs e )
        {
            ProcessAccept(e);
        }

        public void StartReceive(UserToken token)
        {

            try
            {
                //用户连接对象，开始异步接收数据
                bool result = token.conn.ReceiveAsync(token.receiveSAEA);
                if (!result)
                {
                    ProcessReceive(token.receiveSAEA);
                }
            }catch(Exception e)
            {
                Console.WriteLine("23"+e.Message);
            }
        }

        public void IO_Comleted(object sender, SocketAsyncEventArgs e)
        {
            if(e.LastOperation == SocketAsyncOperation.Receive)
            {
                ProcessReceive(e);
            }
            else
            {
                ProcessSend(e);
            }
           
        }

        public void ProcessReceive(SocketAsyncEventArgs e)
        {
            UserToken token = e.UserToken as UserToken;
            //判断网络消息是否接收成功
            if(token.receiveSAEA.BytesTransferred > 0 && token.receiveSAEA.SocketError == SocketError.Success)
            {
                byte[] message = new byte[token.receiveSAEA.BytesTransferred];
                Buffer.BlockCopy(token.receiveSAEA.Buffer, 0, message, 0, token.receiveSAEA.BytesTransferred);
                //处理接收到的消息
                token.receive(message);

                StartReceive(token);
            }
            else
            {
                if(token.receiveSAEA.SocketError != SocketError.Success)
                {
                    ClientClose(token, token.receiveSAEA.SocketError.ToString());
                }
                else
                {
                    ClientClose(token, "客户端主动断开连接");
                } 
            }
             

        }
        public void ProcessSend(SocketAsyncEventArgs e)
        {
            UserToken token = e.UserToken as UserToken;
            if(e.SocketError != SocketError.Success)
            {
                ClientClose(token, e.SocketError.ToString());
            }
            else
            {
                //消息发送成功，回调成功
                token.writed();
            }

        }
        /// <summary>
        /// 客户端断开连接
        /// </summary>
        /// <param name="token">断开用户连接的对象</param>
        /// <param name="error">断开连接的错误编码</param>
        public void ClientClose(UserToken token, string error)
        {
            if(token.conn != null)
            {
                lock (token)
                {
                    //通知应用层面,客户端断开连接了
                    center.ClientClose(token, error);
                    token.Close();
                    //加回一个信号量，供其他用户使用
                    pool.push(token);
                    acceptClients.Release();
                }
            }
        }


    }
}
