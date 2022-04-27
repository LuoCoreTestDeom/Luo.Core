using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luo.Core.Common
{
    public static class SessionHelper
    {


        /// <summary>
        /// 根据session名获取session对象
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T GetSession<T>(this HttpContext context, string keyName)
        {
            var byteArray = context.Session.Get(keyName);
            return byteArray.ByteArrayToObject<T>();
        }
        /// <summary>
        /// 设置session
        /// </summary>
        /// <param name="name">session 名</param>
        /// <param name="val">session 值</param>
        public static void SetSession(this HttpContext context, string name, object val)
        {
            context.Session.Remove(name);
            var bytes = val.ObjToBytesArray();
            context.Session.Set(name, bytes);
        }


        /// <summary>
        /// 根据session名获取session对象
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static int GetSessionInt(this HttpContext context, string keyName)
        {
            return context.Session.GetInt32(keyName).ObjToInt();
        }
        /// <summary>
        /// 设置session
        /// </summary>
        /// <param name="name">session 名</param>
        /// <param name="val">session 值</param>
        public static void SetSessionInt(this HttpContext context, string name, int val)
        {
            context.Session.Remove(name);
            context.Session.SetInt32(name, val);
        }


        /// <summary>
        /// 根据session名获取session对象
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetSessionString(this HttpContext context, string keyName)
        {
            return context.Session.GetString(keyName);
        }
        /// <summary>
        /// 设置session
        /// </summary>
        /// <param name="name">session 名</param>
        /// <param name="val">session 值</param>
        public static void SetSessionString(this HttpContext context, string name, string val)
        {
            context.Session.Remove(name);
            context.Session.SetString(name, val);
        }


        /// <summary>
        /// 清空所有的Session
        /// </summary>
        /// <returns></returns>
        public static void ClearSession(this HttpContext context)
        {
            context.Session.Clear();
        }

        /// <summary>
        /// 删除一个指定的ession
        /// </summary>
        /// <param name="name">Session名称</param>
        /// <returns></returns>
        public static void RemoveSession(this HttpContext context,string name)
        {
            context.Session.Remove(name);
        }

     
    }
}
