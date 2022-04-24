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
            string xmlKeys, xmlPublicKey;
            RSAUtil.RSA_Key(out xmlKeys, out xmlPublicKey);
            var dddd2= RSAUtil.RSA_Encrypt(xmlPublicKey, encryptStr);
            var  res22 = RSAUtil.RSA_Decrypt(xmlKeys, dddd2);
            return RSAUtil.RSA_Encrypt(xmlPublicKey, encryptStr);
        }
        /// <summary>
        /// 解密加过密的字符串
        /// </summary>
        /// <param name="input"></param>
        /// <param name="throwException">解密失败是否抛异常</param>
        /// <returns></returns>
        public static string DecryptString(string input, bool throwException)
        {
            string res = "";
            try
            {
                string xmlKeys, xmlPublicKey;
                RSAUtil.RSA_Key(out xmlKeys, out xmlPublicKey);
                res = RSAUtil.RSA_Decrypt(xmlKeys, input);
                if (MD5Util.ValidateValue(res))
                {
                    return MD5Util.RemoveMD5Profix(Base64Util.Decrypt(MD5Util.RemoveMD5Profix(res)));
                }
                else
                {
                    throw new Exception("字符串无法转换成功！");
                }
            }
            catch
            {
                if (throwException)
                {
                    throw;
                }
                else
                {
                    return "";
                }
            }
        }
    }
}
