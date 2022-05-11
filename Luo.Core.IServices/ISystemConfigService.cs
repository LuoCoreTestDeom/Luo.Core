using Luo.Core.Models.ViewModels.Request;
using Luo.Core.Models.ViewModels.Response;
using Luo.Core.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luo.Core.Models.Dtos.Request;
using Luo.Core.Models.Dtos.Response;

namespace Luo.Core.IServices
{
    public interface ISystemConfigService
    {
        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public CommonViewModel<UserInfoList> QueryUserInfoList(UserInfoQuery req);

        /// <summary>
        /// 添加用户信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public CommonViewModel AddUser(UserInfoInput req);

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public CommonViewModel UpdateUser(UserInfoInput req);

        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public CommonViewModel DeleteUserByUserIds(List<int> userIds);
        /// <summary>
        /// 获取用户的角色
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<UserRoleInfoResult> GetUserRoleByUserId(int userId);





        /// <summary>
        /// 获取菜单信息
        /// </summary>
        /// <returns></returns>
        public CommonViewModel<List<MenuInfoList>> GetMenuInfoList();

        /// <summary>
        /// 获取所有菜单列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<MenuGroupInfoResult> GetMenuInfos();

        /// <summary>
        /// 添加修改菜单
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public CommonViewModel AddEditMenuInfo(MenuInfoInput req);
        /// <summary>
        /// 通过IDs删除菜单信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public CommonViewModel DeleteMenuInfoByIds(List<int> req);

        /// <summary>
        /// 查询角色信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public CommonPageViewModel<List<RoleInfoList>> QueryRolePage(RoleInfoPageQuery req);

        /// <summary>
        /// 查询角色菜单
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public List<TreeRoleMenuResult> QueryRoleMenuInfo(int roleId);
        /// <summary>
        /// 编辑或新增一个角色
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public CommonViewModel AddEditRoleInfo(AddEditRoleInfoInput req);

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public CommonViewModel DeleteRoleInfoByIds(List<int> roleIds);
    }
}
