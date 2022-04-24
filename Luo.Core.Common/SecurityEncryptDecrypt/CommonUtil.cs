using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luo.Core.Common.SecurityEncryptDecrypt
{
    public static class CommonUtil
    {
        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string EncryptString(string input)
        {
            string encryptStr= MD5Util.AddMD5Profix(Base64Util.Encrypt(MD5Util.AddMD5Profix(input)));
            return AESUtil.AES_Encrypt(encryptStr); 
        }
        /// <summary>
        /// 解密加过密的字符串
        /// </summary>
        /// <param name="input"></param>
        /// <param name="throwException">解密失败是否抛异常</param>
        /// <returns></returns>
        public static string DecryptString(string input)
        {
            try
            {
                string res  = AESUtil.AES_Decrypt(input); ;
                if (MD5Util.ValidateValue(res))
                {
                    return MD5Util.RemoveMD5Profix(Base64Util.Decrypt(MD5Util.RemoveMD5Profix(res)));
                }
                else
                {
                    throw new Exception("字符串无法转换成功！");
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
