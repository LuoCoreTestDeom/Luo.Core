using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luo.Core.Common
{
    public static class CookieHelper
    {
        public static void SetCookies(this HttpContext httpContext, string key, string value, int minutes = 30)
        {
            httpContext.Response.Cookies.Append(key, value, new CookieOptions
            {
                Expires = DateTime.Now.AddMinutes(minutes)
            });
        }
        public static void DeleteCookies(this HttpContext httpContext, string key)
        {
            httpContext.Response.Cookies.Delete(key);
        }
        public static string GetCookiesValue(this HttpContext httpContext, string key)
        {
            httpContext.Request.Cookies.TryGetValue(key, out string value);
            return value;
        }
        public static T GetCookie<T>(this HttpContext httpContext,string key)
        {
            httpContext.Request.Cookies.TryGetValue(key, out string sUser);
            if (sUser == null)
            {
                return default(T);
            }
            else
            {
                T res = sUser.JsonToObj<T>();
                return res;
            }
        }
     
    }
}
