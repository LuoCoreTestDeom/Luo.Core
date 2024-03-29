﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luo.Core.Common.SecurityEncryptDecrypt
{
    public static class Base64Util
    {
        /// <summary>
        /// Base64是一種使用64基的位置計數法。它使用2的最大次方來代表僅可列印的ASCII 字元。
        /// 這使它可用來作為電子郵件的傳輸編碼。在Base64中的變數使用字元A-Z、a-z和0-9 ，
        /// 這樣共有62個字元，用來作為開始的64個數字，最後兩個用來作為數字的符號在不同的
        /// 系統中而不同。
        /// Base64加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Base64Encrypt(string str)
        {
            byte[] encbuff = System.Text.Encoding.UTF8.GetBytes(str);
            return Convert.ToBase64String(encbuff);
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Base64Decrypt(string str)
        {
            byte[] decbuff = Convert.FromBase64String(str);
            return System.Text.Encoding.UTF8.GetString(decbuff);
        }
        /// <summary>
        /// 使用默认的密码表加密字符串
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Encrypt(string input)
        {
            return Base64Encode(input);
        }
        /// <summary>
        /// 使用默认的密码表解密字符串
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Decrypt(string input)
        {
            return Base64Decode(input);
        }
        
        #region 获取具有标准的Base64密码表的加密类
        static string Base6464Pad = "=";
        static string Base64CodeTable = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
        #endregion 获取具有标准的Base64密码表的加密类

        static string _MCodeTable = @"ABCDEFGHIJKLMNOPQRSTUVWXYZbacdefghijklmnopqrstu_wxyz0123456789*-";
        static string _MPad = "v";
        static Dictionary<int, char> m_t1 = new Dictionary<int, char>();
        static Dictionary<char, int> m_t2 = new Dictionary<char, int>();
        /// <summary>
        /// 初始化双向哈西字典
        /// </summary>
        public static void InitDict()
        {
            m_t1.Clear();
            m_t2.Clear();
            m_t2.Add(_MPad[0], -1);
            for (int i = 0; i < _MCodeTable.Length; i++)
            {
                m_t1.Add(i, _MCodeTable[i]);
                m_t2.Add(_MCodeTable[i], i);
            }
        }
        /// <summary>
        /// 密码表
        /// </summary>
        public static string CodeTable
        {
            get { return _MCodeTable; }
            set
            {
                if (value == null)
                {
                    throw new Exception("密码表不能为null");
                }
                else if (value.Length < 64)
                {
                    throw new Exception("密码表长度必须至少为64");
                }
                else
                {
                    ValidateRepeat(value);
                    ValidateEqualPad(value, _MPad);
                    _MCodeTable = value;
                    InitDict();
                }
            }
        }
        /// <summary>
        /// 补码
        /// </summary>
        public static string Pad
        {
            get { return _MPad; }
            set
            {
                if (value == null)
                {
                    throw new Exception("密码表的补码不能为null");
                }
                else if (value.Length != 1)
                {
                    throw new Exception("密码表的补码长度必须为1");
                }
                else
                {
                    ValidateEqualPad(_MCodeTable, value);
                    _MPad = value;
                    InitDict();
                }
            }
        }
        /// <summary>
        /// 返回编码后的字符串
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string Base64Encode(string source)
        {
            if (source == null || source == "")
            {
                return "";
            }
            else
            {
                InitDict();
                StringBuilder sb = new StringBuilder();
                byte[] tmp = System.Text.UTF8Encoding.UTF8.GetBytes(source);
                int remain = tmp.Length % 3;
                int patch = 3 - remain;
                if (remain != 0)
                {
                    Array.Resize(ref tmp, tmp.Length + patch);
                }
                int cnt = (int)Math.Ceiling(tmp.Length * 1.0 / 3);
                for (int i = 0; i < cnt; i++)
                {
                    sb.Append(EncodeUnit(tmp[i * 3], tmp[i * 3 + 1], tmp[i * 3 + 2]));
                }
                if (remain != 0)
                {
                    sb.Remove(sb.Length - patch, patch);
                    for (int i = 0; i < patch; i++)
                    {
                        sb.Append(_MPad);
                    }
                }
                return sb.ToString();
            }

        }
         static string EncodeUnit(params byte[] unit)
        {
            int[] obj = new int[4];
            obj[0] = (unit[0] & 0xfc) >> 2;
            obj[1] = ((unit[0] & 0x03) << 4) + ((unit[1] & 0xf0) >> 4);
            obj[2] = ((unit[1] & 0x0f) << 2) + ((unit[2] & 0xc0) >> 6);
            obj[3] = unit[2] & 0x3f;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < obj.Length; i++)
            {
                sb.Append(GetEC((int)obj[i]));
            }
            return sb.ToString();
        }
        static char GetEC(int code)
        {
            return m_t1[code];//m_codeTable[code];
        }

        /// <summary>
        /// 获得解码字符串
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string Base64Decode(string source)
        {
            if (source == null || source == "")
            {
                return "";
            }
            else
            {
                InitDict();
                List<byte> list = new List<byte>();
                char[] tmp = source.ToCharArray();
                int remain = tmp.Length % 4;
                if (remain != 0)
                {
                    Array.Resize(ref tmp, tmp.Length - remain);
                }
                int patch = source.IndexOf(_MPad);
                if (patch != -1)
                {
                    patch = source.Length - patch;
                }
                int cnt = tmp.Length / 4;
                for (int i = 0; i < cnt; i++)
                {
                    DecodeUnit(list, tmp[i * 4], tmp[i * 4 + 1], tmp[i * 4 + 2], tmp[i * 4 + 3]);
                }
                for (int i = 0; i < patch; i++)
                {
                    list.RemoveAt(list.Count - 1);
                }
                return System.Text.Encoding.UTF8.GetString(list.ToArray());
            }
        }
        static void DecodeUnit(List<byte> byteArr, params char[] chArray)
        {
            int[] res = new int[3];
            byte[] unit = new byte[chArray.Length];
            for (int i = 0; i < chArray.Length; i++)
            {
                unit[i] = FindChar(chArray[i]);
            }
            res[0] = (unit[0] << 2) + ((unit[1] & 0x30) >> 4);
            res[1] = ((unit[1] & 0xf) << 4) + ((unit[2] & 0x3c) >> 2);
            res[2] = ((unit[2] & 0x3) << 6) + unit[3];
            for (int i = 0; i < res.Length; i++)
            {
                byteArr.Add((byte)res[i]);
            }
        }
        static byte FindChar(char ch)
        {
            int pos = m_t2[ch];//m_codeTable.IndexOf(ch);
            return (byte)pos;
        }

        /// <summary>
        /// 检查字符串中的字符是否有重复
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        static void ValidateRepeat(string input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                if (input.LastIndexOf(input[i]) > i)
                {
                    throw new Exception("密码表中含有重复字符：" + input[i]);
                }
            }
        }
        /// <summary>
        /// 检查字符串是否包含补码字符
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pad"></param>
        static void ValidateEqualPad(string input, string pad)
        {
            if (input.IndexOf(pad) > -1)
            {
                throw new Exception("密码表中包含了补码字符：" + pad);
            }
        }

        public static void Test()
        {
            InitDict();
            string test = "abc ABC 你好！◎＃￥％……!@#$%^";
            string encode = Base64Encode("false");
            string decode = Base64Decode(encode);
            Console.WriteLine(encode);
            Console.WriteLine(test == decode);
        }
    }
}
