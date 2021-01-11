using System;
using System.IO;
using System.Text;

namespace JBPROTO
{
    public static class NetHelper
    {
        public static INetComponent createNetComponent(INetReactor reactor)
        {
            return new NetComponentImpl(reactor);
        }

        /// <summary>
        /// CA3加密算法：高效快速，自带校验功能
        /// </summary>
        /// <param name="originContent">原始内容</param>
        /// <param name="randumKey">数字随机key</param>
        /// <returns>密文字符串</returns>
        public static string CA3Encode(string originContent, int randumKey)
        {
            byte[] content = System.Text.Encoding.UTF8.GetBytes(originContent);
            byte[] buffer = new byte[content.Length + 4];
            Array.Copy(BitConverter.GetBytes(randumKey), 0, buffer, 0, 4);
            Array.Copy(content, 0, buffer, 4, content.Length);

            int a = 12347, b = 20809, c = 65536;
            for (int i = 0; i < buffer.Length; ++i)
            {
                randumKey = (randumKey * a + b) % c;
                buffer[i] ^= (byte)(randumKey & 0xff);
            }

            return Convert.ToBase64String(buffer);
        }

        /// <summary>
        /// CA3解密算法：高效快速，自带校验功能
        /// </summary>
        /// <param name="encryptContent">密文</param>
        /// <param name="randumKey">数字随机key</param>
        /// <returns>原始内容</returns>
        public static string CA3Decode(string encryptContent, int randumKey)
        {
            byte[] buffer = Convert.FromBase64String(encryptContent);

            if (buffer.Length > 4)
            {
                int tmpKey = randumKey;

                int a = 12347, b = 20809, c = 65536;
                for (int i = 0; i < buffer.Length; ++i)
                {
                    randumKey = (randumKey * a + b) % c;
                    buffer[i] ^= (byte)(randumKey & 0xff);
                }

                int key = BitConverter.ToInt32(buffer, 0);
                if (key == tmpKey)
                    return System.Text.Encoding.UTF8.GetString(buffer, 4, buffer.Length - 4);
            }

            return string.Empty;
        }

        /// <summary>
        /// 安全地写入字符串，如果超过限制则写入空串！！
        /// </summary>
        public static void SafeWriteString(BinaryWriter bw, string content, int maxLength)
        {
            byte[] bcontent = Encoding.UTF8.GetBytes(content ?? string.Empty);
            if (bcontent.Length >= maxLength)
            {
                bw.Write((ushort)0);
                return;
            }
            bw.Write((ushort)bcontent.Length);
            bw.Write(bcontent);
        }
    }
}
