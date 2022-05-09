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

    }
}