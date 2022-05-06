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
        private readonly IBasicRepository _basicRepository;
        private readonly IHttpContextAccessor _accessor;

        public SystemConfigService(ISqlSugarFactory factory, IMapper mapper, IBasicRepository basicRepository, IHttpContextAccessor accessor) : base(factory, mapper)
        {
            _basicRepository = basicRepository;
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
                var userList = _basicRepository.QueryUserInfoList(_Mapper.Map<Models.Dtos.Request.QueryUserInfoDto>(req));
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
                var userInfo = _accessor.HttpContext.GetCookie<LoginUserInfoDto>("UserInfo");
                reqData.CreateName = userInfo.UserName;
                var resData = _basicRepository.AddUser(reqData);
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
                var resData = _basicRepository.UpdateUser(reqData);
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
                var resData = _basicRepository.DeleteUserByUserIds(userIds);
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
