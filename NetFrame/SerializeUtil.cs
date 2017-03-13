using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace NetFrame
{
    public class SerializeUtil
    {
        /// <summary>
        /// 对象序列化
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] encode(object value)
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bw = new BinaryFormatter();
            //将obj序列化成二进制写入到内存流
            bw.Serialize(ms, value);
            byte[] result = new byte[ms.Length];

            Buffer.BlockCopy(ms.GetBuffer(), 0, result, 0, (int)ms.Length);
            ms.Close();
            return result;
        }
        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object decode(byte[] value)
        {
            //创建编码解码的内存流对象并将需要的反序列化数据写入其中
            MemoryStream ms = new MemoryStream(value);

            BinaryFormatter bw = new BinaryFormatter();
            //将obj序列化成二进制写入到内存流
             object result = bw.Deserialize(ms);
            ms.Close();
            return result;
        }
    }
}
