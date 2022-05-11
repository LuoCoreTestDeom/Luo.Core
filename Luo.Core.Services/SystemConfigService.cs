using Luo.Core.DatabaseFactory;
using Luo.Core.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luo.Core.Models.ViewModels.Request;
using Luo.Core.Models.ViewModels.Response;
using Luo.Core.Models.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Luo.Core.Common;
using Luo.Core.Models.Dtos.Response;
using Luo.Core.Models.Dtos.Request;
using Luo.Core.Models.Dtos;
using Luo.Core.Utility.AutoMapper;
using Org.BouncyCastle.Ocsp;
using Renci.SshNet.Common;


namespace Luo.Core.Services
{
    public class SystemConfigService : SqlSugarRepositoryList<ISqlSugarFactory, IBasicRepository>, IServices.ISystemConfigService
    {

        private readonly IHttpContextAccessor _accessor;

        public SystemConfigService(ISqlSugarFactory factory, IBasicRepository rep, IMapper mapper, IHttpContextAccessor accessor) : base(factory, rep, mapper)
        {
            _accessor = accessor;
        }



        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public CommonViewModel<UserInfoList> QueryUserInfoList(UserInfoQuery req)
        {
            CommonViewModel<UserInfoList> res = new CommonViewModel<UserInfoList>();
            try
            {
                UserInfoListDto userList = _Rep.QueryUserInfoList(req.MapTo<QueryUserInfoDto>());
                res.ResultData = _Mapper.Map<UserInfoList>(userList);
                res.Status = true;
                res.StatusCode = 200;

            }
            catch (Exception ex)
            {
                res.Msg = ex.Message;
            }
            return res;
        }

        /// <summary>
        /// 添加用户信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public CommonViewModel AddUser(UserInfoInput req)
        {

            CommonViewModel res = new CommonViewModel();
            try
            {
                if (!req.Password.Equals(req.ConfirmPassword))
                {
                    res.Status = false;
                    res.Msg = "两次密码输入不一致，请重新输入";
                    return res;
                }
                var reqData = _Mapper.Map<AddUserDto>(req);

                reqData.CreateName = _accessor.HttpContext.User.Claims.SingleOrDefault(x => x.Type == "UserName").Value;
                CommonDto resData = _Rep.AddUser(reqData);
                res = resData.AutoMapForMemberTo<CommonDto, CommonViewModel>(fm =>
                {
                    fm.ForMember(dest => dest.Msg, opts => opts.MapFrom(x => x.Message));
                });
            }
            catch (Exception ex)
            {
                res.Msg = ex.Message;
            }
            return res;
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public CommonViewModel UpdateUser(UserInfoInput req)
        {

            CommonViewModel res = new CommonViewModel();
            try
            {
                if (string.IsNullOrWhiteSpace(req.UserName))
                {
                    res.Status = false;
                    res.Msg = "用户名称不能为空";
                    return res;
                }
                var reqData = _Mapper.Map<UpdateUserDto>(req);
                var resData = _Rep.UpdateUser(reqData);
                res = _Mapper.Map<CommonViewModel>(resData);
            }
            catch (Exception ex)
            {
                res.Msg = ex.Message;
            }
            return res;
        }

        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public CommonViewModel DeleteUserByUserIds(List<int> userIds)
        {
            CommonViewModel res = new CommonViewModel();
            try
            {
                var resData = _Rep.DeleteUserByUserIds(userIds);
                res = _Mapper.Map<CommonViewModel>(resData);
            }
            catch (Exception ex)
            {
                res.Msg = ex.Message;
            }
            return res;
        }

        /// <summary>
        /// 获取用户的角色
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<UserRoleInfoResult> GetUserRoleByUserId(int userId)
        {
            List<UserRoleInfoResult> res = new List<UserRoleInfoResult>();
            var roleResData = _Rep.QueryAllRoleInfos();
            res = roleResData.MapToList<UserRoleInfoResult>();
            var userRoleData = _Rep.GetUserRoleIdsByUserId(userId);
            var checkRoles = res.Where(x => userRoleData.Contains(x.RoleId)).ToList();
            foreach (var item in checkRoles)
            {
                item.SelectCheck = true;
            }
            return res;
        }


        /// <summary>
        /// 获取菜单信息
        /// </summary>
        /// <returns></returns>
        public CommonViewModel<List<MenuInfoList>> GetMenuInfoList()
        {
            CommonViewModel<List<MenuInfoList>> res = new CommonViewModel<List<MenuInfoList>>();
            try
            {
                var resData = _Rep.QueryMenuList();
                res.ResultData = _Mapper.Map<List<MenuInfoDto>, List<MenuInfoList>>(resData);
                res.Status = true;
                res.StatusCode = 0;
            }
            catch (Exception ex)
            {
                res.Msg = ex.Message;
            }
            return res;

        }


        /// <summary>
        /// 获取所有菜单列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<MenuGroupInfoResult> GetMenuInfos()
        {
            var resData = _Rep.QueryAllMenuInfoList();
            resData.Add(new MenuInfoDto()
            {
                MenuId = 0,
                MenuName = "顶级",
                ParentMenuId = -1,
            });
            return RecursionMenu(resData.Where(x => x.ParentMenuId == -1).ToList(), resData);
        }
        /// <summary>
        /// 递归遍历菜单
        /// </summary>
        /// <param name="resData"></param>
        /// <param name="sourceData"></param>
        /// <returns></returns>
        private List<MenuGroupInfoResult> RecursionMenu(List<MenuInfoDto> resData, List<MenuInfoDto> sourceData)
        {
            List<MenuGroupInfoResult> res = new List<MenuGroupInfoResult>();
            foreach (var item in resData)
            {
                var menuInfo = _Mapper.Map<MenuGroupInfoResult>(item);
                menuInfo.Children = RecursionMenu(sourceData.Where(x => x.ParentMenuId == item.MenuId).ToList(), sourceData);
                res.Add(menuInfo);
            }
            return res;
        }

        /// <summary>
        /// 添加修改菜单
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public CommonViewModel AddEditMenuInfo(MenuInfoInput req)
        {
            CommonViewModel res = new CommonViewModel();
            try
            {
                if (req.MenuId > 0)
                {
                    res.Status = _Rep.EditMenuInfo(_Mapper.Map<EditMenuInfoDto>(req));
                }
                else
                {
                    res.Status = _Rep.AddMenuInfo(_Mapper.Map<AddMenuInfoDto>(req));
                }

            }
            catch (Exception ex)
            {
                res.Msg = ex.Message;
            }
            return res;
        }


        /// <summary>
        /// 通过IDs删除菜单信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public CommonViewModel DeleteMenuInfoByIds(List<int> req)
        {
            CommonViewModel res = new CommonViewModel();
            try
            {
                var resData = _Rep.DeleteMenuInfoByIds(req);
                res = _Mapper.Map<CommonViewModel>(resData);
            }
            catch (Exception ex)
            {
                res.Msg = ex.Message;
            }
            return res;
        }


        /// <summary>
        /// 查询角色信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public CommonPageViewModel<List<RoleInfoList>> QueryRolePage(RoleInfoPageQuery req)
        {
            CommonPageViewModel<List<RoleInfoList>> res = new CommonPageViewModel<List<RoleInfoList>>();
            try
            {
                var reqData = req.MapTo<QueryRoleInfoDto>();
                CommonPageDto<List<RoleInfoDto>> resData = _Rep.QueryRoleInfo(reqData);

                res = _Mapper.Map<CommonPageDto<List<RoleInfoDto>>, CommonPageViewModel<List<RoleInfoList>>>(resData);
                res.StatusCode = 200;
            }
            catch (Exception ex)
            {
                res.Msg = ex.Message;
            }
            return res;
        }


        /// <summary>
        /// 查询角色菜单
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public List<TreeRoleMenuResult> QueryRoleMenuInfo(int roleId)
        {
            var resData = _Rep.QueryAllMenuInfoList();
            resData.Add(new MenuInfoDto()
            {
                MenuId = 0,
                MenuName = "顶级",
                ParentMenuId = -1,
            });
            var roleMenuIds = _Rep.QueryRoleMenuIds(roleId);
            return RecursionRoleMenu(resData.Where(x => x.ParentMenuId == -1).ToList(), resData, roleMenuIds);
        }

        /// <summary>
        /// 递归遍历菜单
        /// </summary>
        /// <param name="resData"></param>
        /// <param name="sourceData"></param>
        /// <returns></returns>
        private List<TreeRoleMenuResult> RecursionRoleMenu(List<MenuInfoDto> resData, List<MenuInfoDto> sourceData, List<int> roleMenuIds)
        {
            List<TreeRoleMenuResult> res = new List<TreeRoleMenuResult>();
            foreach (var item in resData)
            {
                var menuInfo = _Mapper.Map<TreeRoleMenuResult>(item);

                menuInfo.Spread = true;
                menuInfo.Children = RecursionRoleMenu(sourceData.Where(x => x.ParentMenuId == item.MenuId).ToList(), sourceData, roleMenuIds);
                if (menuInfo.Children == null || menuInfo.Children.Count < 1)
                {
                    if (roleMenuIds.Any(x => x == menuInfo.Id))
                    {
                        menuInfo.@Checked = true;
                    }
                }
                res.Add(menuInfo);
            }
            return res;
        }

        /// <summary>
        /// 编辑或新增一个角色
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public CommonViewModel AddEditRoleInfo(AddEditRoleInfoInput req)
        {
            CommonViewModel res = new CommonViewModel();
            List<int> menuIds = GetMenuIdsByRoleMenuInfos(req.RoleMenuInfos);
            if (req.RoleId > 0)
            {
                var resData = _Rep.EditRoleInfo(new UpdateRoleInfoDto()
                {
                    RoleId = req.RoleId,
                    RoleName = req.RoleName,
                    MenuIds = menuIds,
                });
                res = _Mapper.Map<CommonViewModel>(resData);
            }
            else
            {
                var resData = _Rep.AddRoleInfo(new AddRoleInfoDto()
                {
                    RoleName = req.RoleName,
                    MenuIds = menuIds,
                });
                res = _Mapper.Map<CommonViewModel>(resData);
            }
            return res;
        }
        /// <summary>
        /// 取出递归中的菜单ID
        /// </summary>
        /// <param name="roleMenuInfos"></param>
        /// <returns></returns>
        private List<int> GetMenuIdsByRoleMenuInfos(List<TreeRoleMenuResult> roleMenuInfos)
        {
            List<int> res = new List<int>();

            if (roleMenuInfos != null && roleMenuInfos.Count > 0)
            {
                foreach (var item in roleMenuInfos)
                {
                    if (item.Id > 0)
                    {
                        res.Add(item.Id);
                    }

                    if (item.Children != null && item.Children.Count > 0)
                    {
                        res.AddRange(GetMenuIdsByRoleMenuInfos(item.Children));
                    }

                }
            }
            return res;
        }
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public CommonViewModel DeleteRoleInfoByIds(List<int> roleIds)
        {
            var resData = _Rep.DeleteRoleInfoByIds(roleIds);
            return _Mapper.Map<CommonViewModel>(resData);
        }
    }
}
