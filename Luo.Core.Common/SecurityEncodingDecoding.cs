using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Luo.Core.Common
{
    public class SecurityEncodingDecoding
    {
        #region DES对称加密解密
        public static  string DEFAULT_ENCRYPT_KEY = Appsettings.GetValue("DesKey");
        /// <summary>
        /// 使用默认加密
        /// </summary>
        /// <param name="strText"></param>
        /// <returns></returns>
        public static string DesEncrypt(string strText)
        {
            try
            {
                return DesEncrypt(strText, DEFAULT_ENCRYPT_KEY);
            }
            catch
            {
                return "";
            }
        }
        /// <summary>
        /// 使用默认解密
        /// </summary>
        /// <param name="strText"></param>
        /// <returns></returns>
        public static string DesDecrypt(string strText)
        {
            try
            {
                return DesDecrypt(strText, DEFAULT_ENCRYPT_KEY);
            }
            catch
            {
                return "";
            }
        }
        /// <summary> 
        /// Encrypt the string 
        /// Attention:key must be 8 bits 
        /// </summary> 
        /// <param name="strText">string</param> 
        /// <param name="strEncrKey">key</param> 
        /// <returns></returns> 
        public static string DesEncrypt(string strText, string strEncrKey)
        {
            byte[] byKey = null;
            byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

            byKey = Encoding.UTF8.GetBytes(strEncrKey.Substring(0, 8));
            DES des = DES.Create();
            byte[] inputByteArray = Encoding.UTF8.GetBytes(strText);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Convert.ToBase64String(ms.ToArray());
        }
        /// <summary> 
        /// Decrypt string 
        /// Attention:key must be 8 bits 
        /// </summary> 
        /// <param name="strText">Decrypt string</param> 
        /// <param name="sDecrKey">key</param> 
        /// <returns>output string</returns> 
        public static string DesDecrypt(string strText, string sDecrKey)
        {
            byte[] byKey = null;
            byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
            byte[] inputByteArray = new Byte[strText.Length];

            byKey = Encoding.UTF8.GetBytes(sDecrKey.Substring(0, 8));
            DES des = DES.Create();
            inputByteArray = Convert.FromBase64String(strText);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            Encoding encoding = new UTF8Encoding();
            return encoding.GetString(ms.ToArray());
        }
        /// <summary> 
        /// Encrypt files 
        /// Attention:key must be 8 bits 
        /// </summary> 
        /// <param name="m_InFilePath">Encrypt file path</param> 
        /// <param name="m_OutFilePath">output file</param> 
        /// <param name="strEncrKey">key</param> 
        public static void DesEncrypt(string m_InFilePath, string m_OutFilePath, string strEncrKey)
        {
            byte[] byKey = null;
            byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

            byKey = Encoding.UTF8.GetBytes(strEncrKey.Substring(0, 8));
            FileStream fin = new FileStream(m_InFilePath, FileMode.Open, FileAccess.Read);
            FileStream fout = new FileStream(m_OutFilePath, FileMode.OpenOrCreate, FileAccess.Write);
            fout.SetLength(0);
            //Create variables to help with read and write. 
            byte[] bin = new byte[100]; //This is intermediate storage for the encryption. 
            long rdlen = 0; //This is the total number of bytes written. 
            long totlen = fin.Length; //This is the total length of the input file. 
            int len; //This is the number of bytes to be written at a time. 

            DES des = DES.Create(); 
            CryptoStream encStream = new CryptoStream(fout, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write);

            //Read from the input file, then encrypt and write to the output file. 
            while (rdlen < totlen)
            {
                len = fin.Read(bin, 0, 100);
                encStream.Write(bin, 0, len);
                rdlen = rdlen + len;
            }
            encStream.Close();
            fout.Close();
            fin.Close();
        }
        /// <summary> 
        /// Decrypt files 
        /// Attention:key must be 8 bits 
        /// </summary> 
        /// <param name="m_InFilePath">Decrypt filepath</param> 
        /// <param name="m_OutFilePath">output filepath</param> 
        /// <param name="sDecrKey">key</param> 
        public static void DesDecrypt(string m_InFilePath, string m_OutFilePath, string sDecrKey)
        {
            byte[] byKey = null;
            byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

            byKey = Encoding.UTF8.GetBytes(sDecrKey.Substring(0, 8));
            FileStream fin = new FileStream(m_InFilePath, FileMode.Open, FileAccess.Read);
            FileStream fout = new FileStream(m_OutFilePath, FileMode.OpenOrCreate, FileAccess.Write);
            fout.SetLength(0);
            //Create variables to help with read and write. 
            byte[] bin = new byte[100]; //This is intermediate storage for the encryption. 
            long rdlen = 0; //This is the total number of bytes written. 
            long totlen = fin.Length; //This is the total length of the input file. 
            int len; //This is the number of bytes to be written at a time. 

            DES des = DES.Create();
            CryptoStream encStream = new CryptoStream(fout, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write);

            //Read from the input file, then encrypt and write to the output file. 
            while (rdlen < totlen)
            {
                len = fin.Read(bin, 0, 100);
                encStream.Write(bin, 0, len);
                rdlen = rdlen + len;
            }
            encStream.Close();
            fout.Close();
            fin.Close();
        }
        #endregion DES对称加密解密
    }
}
