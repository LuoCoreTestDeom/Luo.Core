using Luo.Core.Models.ViewModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luo.Core.IServices
{
    public interface ISharedService
    {
        /// <summary>
        /// 获取用户的菜单列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<UserMenuInfoOutput> GetUserMenuInfos(int userId);
    }
}
