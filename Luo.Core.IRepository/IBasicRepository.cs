using Luo.Core.DatabaseEntity;
using Luo.Core.DatabaseFactory;
using Luo.Core.Models.Dtos;
using Luo.Core.Models.Dtos.Request;
using Luo.Core.Models.Dtos.Response;

namespace Luo.Core.IRepository
{
    public interface IBasicRepository : ISqlSugarRepository
    {
       
        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <returns></returns>
        public LoginUserInfoDto QueryUserInfo(LoginUserDto req);
        /// <summary>
        /// 查询所有用户信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public UserInfoListDto QueryUserInfoList(QueryUserInfoDto req);
        /// <summary>
        /// 添加用户信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public CommonDto AddUser(AddUserDto req);
        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public CommonDto UpdateUser(UpdateUserDto req);
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        public CommonDto DeleteUserByUserIds(List<int> userIds);
        /// <summary>
        /// 获取用户绑定的所有角色
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<int> GetUserRoleIdsByUserId(int userId);




        /// <summary>
        /// 查询菜单列表信息
        /// </summary>
        /// <returns></returns>
        public List<MenuInfoDto> QueryMenuList();

        /// <summary>
        /// 通过用户ID获取菜单列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<MenuInfoDto> QueryMenuInfoListUserId(int userId);

        /// <summary>
        /// 获取所有菜单列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<MenuInfoDto> QueryAllMenuInfoList();

        /// <summary>
        /// 添加菜单
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool AddMenuInfo(AddMenuInfoDto req);

        /// <summary>
        /// 修改菜单
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool EditMenuInfo(EditMenuInfoDto req);
        /// <summary>
        /// 通过菜单ID 删除菜单信息
        /// </summary>
        /// <param name="menuIds"></param>
        /// <returns></returns>
        public CommonDto DeleteMenuInfoByIds(List<int> menuIds);
        /// <summary>
        /// 查询角色信息
        /// </summary>
        /// <param name="menuIds"></param>
        /// <returns></returns>
        public CommonPageDto<List<RoleInfoDto>> QueryRoleInfo(QueryRoleInfoDto req);

        /// <summary>
        /// 通过角色ID 获取菜单Ids
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public List<int> QueryRoleMenuIds(int roleId);


        /// <summary>
        /// 新增一个角色
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public CommonDto AddRoleInfo(AddRoleInfoDto req);
        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public CommonDto EditRoleInfo(UpdateRoleInfoDto req);

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public CommonDto DeleteRoleInfoByIds(List<int> roleIds);
        /// <summary>
        /// 查询所有角色信息
        /// </summary>
        /// <returns></returns>
        public List<QueryAllRoleInfoDto> QueryAllRoleInfos();

        /// <summary>
        /// 查询会员信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public CommonPageDto<List<MemberInfoListDto>> QueryMemberInfoPageList(QueryMemberInfoPageDto req);

    }
}