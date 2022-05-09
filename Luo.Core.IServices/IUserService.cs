using Luo.Core.Models.Dtos.Response;
using Luo.Core.Models.ViewModels;
using Luo.Core.Models.ViewModels.Request;
using Luo.Core.Models.ViewModels.Response;

namespace Luo.Core.IServices
{
    public interface IUserService
    {
     
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public CommonViewModel<LoginUserInfoDto> UserLogin(LoginUserForm req);
        /// <summary>
        /// 获取用户的菜单列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<MenuInfoList> GetUserMenuInfos(int userId);
        /// <summary>
        /// 递归遍历菜单
        /// </summary>
        /// <param name="resData"></param>
        /// <param name="sourceData"></param>
        /// <returns></returns>
        public List<UserMenuInfoOutput> RecursionMenu(List<MenuInfoList> resData, List<MenuInfoList> sourceData);
    }
}