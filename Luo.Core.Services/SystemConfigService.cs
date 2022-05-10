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
                var userList = _Rep.QueryUserInfoList(_Mapper.Map<Models.Dtos.Request.QueryUserInfoDto>(req));
                res.Status = true;
                res.StatusCode = 200;
                res.ResultData = _Mapper.Map<UserInfoList>(userList);
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
                var resData = _Rep.AddUser(reqData);
                res = _Mapper.Map<CommonViewModel>(resData);
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
        /// 获取菜单信息
        /// </summary>
        /// <returns></returns>
        public CommonViewModel<List<MenuInfoList>> GetMenuInfoList() 
        {
            CommonViewModel<List<MenuInfoList>> res = new CommonViewModel<List<MenuInfoList>>();
            try
            {
                var resData = _Rep.QueryMenuList();
                res.ResultData= _Mapper.Map<List<MenuInfoDto>, List<MenuInfoList>>(resData);
                res.Status= true;
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
                MenuId=0,
                MenuName="顶级",
                ParentMenuId=-1,
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
               res.Msg=ex.Message;
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
               var resData=_Rep.DeleteMenuInfoByIds(req);
                res = _Mapper.Map<CommonViewModel>(resData);
            }
            catch (Exception ex)
            {
                res.Msg = ex.Message;
            }
            return res;
        }

    }
}
