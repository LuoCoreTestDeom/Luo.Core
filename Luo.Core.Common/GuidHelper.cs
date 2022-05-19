using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Luo.Core.Common
{
    public static class GuidHelper
    {
        public static bool IsGuidByParse(this string strSrc)
        {
            Guid g = Guid.Empty;
            return Guid.TryParse(strSrc, out g);
        }

        public static bool IsGuidByRegNoComplied(this string strSrc)
        {
            Regex reg = new Regex("^[A-F0-9]{8}(-[A-F0-9]{4}){3}-[A-F0-9]{12}$");
            return reg.IsMatch(strSrc);
        }
        public static bool IsGuidByArr(this string strSrc)
        {
            if (String.IsNullOrEmpty(strSrc) || strSrc.Length != 36) { return false; }
            string[] arr = strSrc.Split('-');
            if (arr.Length != 5) { return false; }
            for (int i = 0; i < arr.Length; i++)
            {
                for (int j = 0; j < arr[i].Length; j++)
                {
                    char a = arr[i][j];
                    if (!((a >= 48 && a <= 57) || (a >= 65 && a <= 90) || (a >= 97 && a <= 122)))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
