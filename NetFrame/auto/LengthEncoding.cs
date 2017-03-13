using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NetFrame.auto
{
    public class LengthEncoding
    {
        public static byte[] encode(byte[] buff)
        {
            MemoryStream ms = new MemoryStream();//创建内存刘对象
            BinaryWriter sw = new BinaryWriter(ms);//写入二进制对象流
            sw.Write(buff.Length);
            sw.Write(buff);
            byte[] result = new byte[ms.Length];
            Buffer.BlockCopy(ms.GetBuffer(), 0, result, 0, (int)ms.Length);

            sw.Close();
            ms.Close();

            return result;  
        }
        /// <summary>
        /// 粘包长度解码
        /// </summary>
        /// <param name="cache"></param>
        /// <returns></returns>
        public static byte[] decode(ref List<byte> cache)
        {
            if (cache.Count < 4) return null;

            MemoryStream ms = new MemoryStream(cache.ToArray());//创建内存流对象，并将缓存数据写入进去
            BinaryReader br = new BinaryReader(ms);//二进制读取流
            int length = br.ReadInt32();//从缓存中读取int型消息体长度
            //如果消息长度大于缓存中数据长度，说明消息还没读取完，等待下次消息到达后再次处理
            if(length > ms.Length - ms.Position)
            {
                return null;
            }
            //读取正确长度的数据
            byte[] result = br.ReadBytes(length);
            //清空缓存
            cache.Clear();
            //将读取后剩余的数据写入缓存
            cache.AddRange(br.ReadBytes((int)(ms.Length - ms.Position)));
            br.Close();
            ms.Close();
            return result;
        }

    }
}
