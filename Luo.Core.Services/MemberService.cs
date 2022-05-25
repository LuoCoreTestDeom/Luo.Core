using AutoMapper;
using Luo.Core.DatabaseFactory;
using Luo.Core.IRepository;
using Luo.Core.Models.Dtos;
using Luo.Core.Models.Dtos.Request;
using Luo.Core.Models.Dtos.Response;
using Luo.Core.Models.ViewModels;
using Luo.Core.Models.ViewModels.Request;
using Luo.Core.Models.ViewModels.Response;
using Luo.Core.Utility.AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luo.Core.Services
{
    public class MemberService : SqlSugarRepositoryList<ISqlSugarFactory, IMemberRepository>, IServices.IMemberService
    {
        private readonly IHttpContextAccessor _accessor;
        public MemberService(ISqlSugarFactory factory, IMemberRepository rep, IMapper mapper, IHttpContextAccessor accessor) : base(factory, rep, mapper)
        {
            _accessor = accessor;
        }
        /// <summary>
        /// 查询会员信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public CommonPageViewModel<List<MemberInfoResult>> QueryMemberInfoPageList(MemberInfoPageQuery req)
        {
            var resData = _Rep.QueryMemberInfoPageList(req.AutoMapTo<QueryMemberInfoPageDto>());
            CommonPageViewModel<List<MemberInfoResult>> res = new CommonPageViewModel<List<MemberInfoResult>>();
            res = _Mapper.Map<CommonPageDto<List<MemberInfoListDto>>, CommonPageViewModel<List<MemberInfoResult>>>(resData);
            return res;
        }
        /// <summary>
        /// 添加编辑会员信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public CommonViewModel AddEditMemberInfo(AddEditMemberInfoInput req)
        {
            CommonViewModel res = new CommonViewModel();
            if (string.IsNullOrWhiteSpace(req.MemberName))
            {
                res.Msg = "用户名称不能为空！";
                return res;
            }

            if (req.MemberId < 1)
            {
                if (string.IsNullOrWhiteSpace(req.Password))
                {
                    res.Msg = "密码不能为空！";
                    return res;
                }
                if (!req.Password.Equals(req.MemberConfirmPassword))
                {
                    res.Msg = "两次密码不对应！";
                    return res;
                }
                var reqData = req.MapTo<AddMemberInfoDto>();
                reqData.Password = Luo.Core.Common.SecurityEncryptDecrypt.CommonUtil.EncryptString(reqData.Password);
                reqData.CreateName = _accessor.HttpContext.User.Claims.SingleOrDefault(x => x.Type == "UserName").Value;
                var resData = _Rep.AddMemberInfo(reqData);
                res = _Mapper.Map<CommonViewModel>(resData);
            }
            else
            {
                var memberPassword = _Rep.GetMemberPasswordByMemberId(req.MemberId);
                if (string.IsNullOrWhiteSpace(memberPassword))
                {
                    res.Msg = "无法获取会员信息！";
                    return res;
                }
                memberPassword = Common.SecurityEncryptDecrypt.CommonUtil.DecryptString(memberPassword);

                var reqData = req.MapTo<UpdateMemberInfoDto>();
                if (!string.IsNullOrWhiteSpace(req.Password))
                {
                    if (!memberPassword.Equals(req.MemberConfirmPassword))
                    {
                        res.Msg = "原密码不正确!";
                        return res;
                    }
                    reqData.Password = Luo.Core.Common.SecurityEncryptDecrypt.CommonUtil.EncryptString(reqData.Password);
                }
                var resData = _Rep.UpdateMemberInfo(reqData);
                res = _Mapper.Map<CommonViewModel>(resData);
            }
            return res;

        }

        /// <summary>
        /// 删除会员信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public CommonViewModel DeleteMemberByMemberId(List<int> req)
        {
            CommonViewModel res = new CommonViewModel();
            var resData = _Rep.DeleteMemberByMemberIds(req);
            res = _Mapper.Map<CommonViewModel>(resData);
            return res;
        }


        /// <summary>
        /// 查询会员信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public CommonViewModel<MemberInfoResult> LoginMemberInfo(MemberLogin req)
        {
            CommonViewModel<MemberInfoResult> res = new CommonViewModel<MemberInfoResult>();
            try
            {
                if(req==null||string.IsNullOrWhiteSpace(req.Account)|| string.IsNullOrWhiteSpace(req.Password)) 
                {
                    res.Msg = "请求必要的参数不能为空！";
                    return res;
                }
                MemberInfoDto resData = _Rep.QueryJwtMemberInfo(new LoginMemberDto() 
                {
                    MemberName=req.Account,
                    MemberPassword=req.Password
                });
                if (resData == null )
                {
                    return res;
                }
                if (resData.MemberId > 0) 
                {
                    res.ResultData = resData.MapTo<MemberInfoResult>();
                    res.Status = true;
                }
                else 
                {
                    res.Status=false;
                }
            }
            catch (Exception ex)
            {
                res.Msg = ex.Message;
            }
            return res;
        }

    }
}
