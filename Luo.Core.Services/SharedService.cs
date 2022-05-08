using AutoMapper;
using Luo.Core.DatabaseFactory;
using Luo.Core.IRepository;
using Luo.Core.Models.ViewModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luo.Core.Services
{
    public class SharedService : SqlSugarRepositoryList<ISqlSugarFactory, IBasicRepository>, IServices.ISharedService
    {
        public SharedService(ISqlSugarFactory factory, IBasicRepository rep, IMapper mapper) : base(factory, rep, mapper)
        {
        }
        /// <summary>
        /// 获取用户的菜单列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<UserMenuInfoOutput> GetUserMenuInfos(int userId)
        {
            var resData = _Rep.QueryMenuInfoListUserId(userId);
           
            return RecursionMenu(resData.Where(x => x.MenuType == 0).ToList(), resData);
        }
        /// <summary>
        /// 递归遍历菜单
        /// </summary>
        /// <param name="resData"></param>
        /// <param name="sourceData"></param>
        /// <returns></returns>
        private List<UserMenuInfoOutput> RecursionMenu(List<Luo.Core.Models.Dtos.Response.MenuInfoDto> resData, List<Luo.Core.Models.Dtos.Response.MenuInfoDto> sourceData)
        {
            List<UserMenuInfoOutput> res = new List<UserMenuInfoOutput>();
            foreach (var item in resData)
            {
                var menuInfo = _Mapper.Map<UserMenuInfoOutput>(item);
                menuInfo.ChildrenMeuns = RecursionMenu(sourceData.Where(x => x.ParentMenuId == item.MenuId).ToList(), sourceData);
                res.Add(menuInfo);
            }
            return res;
        }
    }
}
