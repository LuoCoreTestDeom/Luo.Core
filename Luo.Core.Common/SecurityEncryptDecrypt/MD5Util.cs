using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Luo.Core.Common.SecurityEncryptDecrypt
{
    public static class MD5Util
    {
        public static string HashPasswordForStoringInConfigFile(this string str) 
        {
            using (var md5 = MD5.Create())
            {
                var result = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
                var strResult = BitConverter.ToString(result);
                return strResult.Replace("-", "").ToUpper();
            }
        }

        /// <summary> 
        /// MD5 Encrypt 
        /// </summary> 
        /// <param name="strText">text</param> 
        /// <returns>md5 Encrypt string</returns> 
        public static string MD5Encrypt(string strText)
        {
            MD5 md5 = MD5.Create();
            byte[] result = md5.ComputeHash(Encoding.Default.GetBytes(strText));
            return Encoding.Default.GetString(result);
        }

        public static string MD5EncryptHash(String input)
        {
            MD5 md5 = MD5.Create();
            //the GetBytes method returns byte array equavalent of a string
            byte[] res = md5.ComputeHash(Encoding.Default.GetBytes(input), 0, input.Length);
            char[] temp = new char[res.Length];
            //copy to a char array which can be passed to a String constructor
            Array.Copy(res, temp, res.Length);
            //return the result as a string
            return new String(temp);
        }

        public static string MD5EncryptHashHex(String input)
        {
            MD5 md5 = MD5.Create();
            //the GetBytes method returns byte array equavalent of a string
            byte[] res = md5.ComputeHash(Encoding.Default.GetBytes(input), 0, input.Length);

            String returnThis = string.Empty;

            for (int i = 0; i < res.Length; i++)
            {
                returnThis += Uri.HexEscape((char)res[i]);
            }
            returnThis = returnThis.Replace("%", "");
            returnThis = returnThis.ToLower();

            return returnThis;
        }

        /// <summary>
        /// MD5 三次加密算法.计算过程: (QQ使用)
        /// 1. 验证码转为大写
        /// 2. 将密码使用这个方法进行三次加密后,与验证码进行叠加
        /// 3. 然后将叠加后的内容再次MD5一下,得到最终验证码的值
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string EncyptMD5_3_16(string s)
        {
            MD5 md5 = MD5.Create();
            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(s);
            byte[] bytes1 = md5.ComputeHash(bytes);
            byte[] bytes2 = md5.ComputeHash(bytes1);
            byte[] bytes3 = md5.ComputeHash(bytes2);

            StringBuilder sb = new StringBuilder();
            foreach (var item in bytes3)
            {
                sb.Append(item.ToString("x").PadLeft(2, '0'));
            }
            return sb.ToString().ToUpper();
        }
        /// <summary>
        /// 获得32位的MD5加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetMD5_32(string input)
        {
            MD5 md5 = MD5.Create();
            byte[] data = md5.ComputeHash(System.Text.Encoding.Default.GetBytes(input));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sb.Append(data[i].ToString("x2"));
            }
            return sb.ToString();
        }
        /// <summary>
        /// 获得16位的MD5加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetMD5_16(string input)
        {
            return GetMD5_32(input).Substring(8, 16);
        }
        /// <summary>
        /// 获得8位的MD5加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetMD5_8(string input)
        {
            return GetMD5_32(input).Substring(8, 8);
        }
        /// <summary>
        /// 获得4位的MD5加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetMD5_4(string input)
        {
            return GetMD5_32(input).Substring(8, 4);
        }
        /// <summary>
        /// 添加MD5的前缀，便于检查有无篡改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string AddMD5Profix(string input)
        {
            return GetMD5_4(input) + input;
        }
        /// <summary>
        /// 移除MD5的前缀
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string RemoveMD5Profix(string input)
        {
            return input.Substring(4);
        }
        /// <summary>
        /// 验证MD5前缀处理的字符串有无被篡改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool ValidateValue(string input)
        {
            bool res = false;
            if (input.Length >= 4)
            {
                string tmp = input.Substring(4);
                if (input.Substring(0, 4) == GetMD5_4(tmp))
                {
                    res = true;
                }
            }
            return res;
        }


        #region MD5签名验证
        /// <summary>
        /// 对给定文件路径的文件加上标签
        /// </summary>
        /// <param name="path">要加密的文件的路径</param>
        /// <returns>标签的值</returns>
        public static bool AddMD5(string path)
        {
            bool IsNeed = true;

            if (CheckMD5(path))                                  //已进行MD5处理
                IsNeed = false;

            try
            {
                FileStream fsread = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                byte[] md5File = new byte[fsread.Length];
                fsread.Read(md5File, 0, (int)fsread.Length);                               // 将文件流读取到Buffer中
                fsread.Close();

                if (IsNeed)
                {
                    string result = MD5Buffer(md5File, 0, md5File.Length);             // 对Buffer中的字节内容算MD5
                    byte[] md5 = System.Text.Encoding.ASCII.GetBytes(result);       // 将字符串转换成字节数组以便写人到文件中
                    FileStream fsWrite = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);
                    fsWrite.Write(md5File, 0, md5File.Length);                               // 将文件，MD5值 重新写入到文件中。
                    fsWrite.Write(md5, 0, md5.Length);
                    fsWrite.Close();
                }
                else
                {
                    FileStream fsWrite = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);
                    fsWrite.Write(md5File, 0, md5File.Length);
                    fsWrite.Close();
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 对给定路径的文件进行验证
        /// </summary>
        /// <param name="path"></param>
        /// <returns>是否加了标签或是否标签值与内容值一致</returns>
        public static bool CheckMD5(string path)
        {
            try
            {
                FileStream get_file = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                byte[] md5File = new byte[get_file.Length];                                      // 读入文件
                get_file.Read(md5File, 0, (int)get_file.Length);
                get_file.Close();

                string result = MD5Buffer(md5File, 0, md5File.Length - 32);             // 对文件除最后32位以外的字节计算MD5，这个32是因为标签位为32位。
                string md5 = System.Text.Encoding.ASCII.GetString(md5File, md5File.Length - 32, 32);   //读取文件最后32位，其中保存的就是MD5值
                return result == md5;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 计算文件的MD5值
        /// </summary>
        /// <param name="MD5File">MD5签名文件字符数组</param>
        /// <param name="index">计算起始位置</param>
        /// <param name="count">计算终止位置</param>
        /// <returns>计算结果</returns>
        private static string MD5Buffer(byte[] MD5File, int index, int count)
        {
            System.Security.Cryptography.MD5 get_md5 = System.Security.Cryptography.MD5.Create();
            byte[] hash_byte = get_md5.ComputeHash(MD5File, index, count);
            string result = System.BitConverter.ToString(hash_byte);

            result = result.Replace("-", "");
            return result;
        }
        #endregion MD5签名验证
        private static void Test()
        {
            string o = "i love u";
            o = AddMD5Profix(o);
            //o += " ";
            Console.WriteLine(o);
            Console.WriteLine(ValidateValue(o));

            o = RemoveMD5Profix(o);
            Console.WriteLine(o);

        }

    }
}
