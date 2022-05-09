using Luo.Core.Common;
using Luo.Core.Models.ViewModels.Response;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luo.Core.Utility
{
    public static class MenuPermissionsHelper
    {
        public static List<MenuInfoList> GetMenuButton(this HttpContext httpContext, string menuUrl)
        {
            List<MenuInfoList> res = null;
            var userClaims = httpContext.User.Claims.SingleOrDefault(x => x.Type == "UserMenuList");
            if (userClaims != null)
            {
                var menuInfos = userClaims.Value.JsonToObj<List<MenuInfoList>>();
                if (menuInfos != null)
                {
                    var userParentId = menuInfos.Where(x => x.MenuUrl.Contains(menuUrl)).FirstOrDefault();
                    if (userParentId != null)
                    {
                        res = menuInfos.Where(x => x.ParentMenuId == userParentId.MenuId).ToList();
                    }
                }
            }
            return res;
        }

      
    }
}
