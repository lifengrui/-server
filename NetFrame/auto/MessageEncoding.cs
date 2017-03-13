using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFrame.auto
{
    public class MessageEncoding
    {
        /// <summary>
        /// 消息体序列化
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] encode(object value){
            SocketModel model = value as SocketModel;
            ByteArray ba = new ByteArray();
            ba.write(model.type);
            ba.write(model.area);
            ba.write(model.command);
            //判断消息体是否为空
            if(model.message != null)
            {
                ba.write(SerializeUtil.encode(model.message));
            }
            
            byte[] result = ba.getBuff();
            ba.Close();
            return result;
        }
        /// <summary>
        /// 消息体反序列化
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object decode(byte[] value){
            ByteArray ba = new ByteArray(value);
            SocketModel model = new SocketModel();
            byte type;
            int area;
            int command;
            //写入顺序与读取数据一致,因为这是自己定的三层协议
            ba.read(out type);
            ba.read(out area);
            ba.read(out command);
            model.type = type;
            model.area = area;
            model.command = command;
            //判断读取完成协议后，是否还有书籍需要读取，是则说明有消息体，进行消息读取 
            if (ba.Readnable)
            {
                byte[] message;
                ba.read(out message,ba.Length - ba.Position);
                model.message = SerializeUtil.decode(message);
            }
            ba.Close();
            return model;
        }
    }
}
