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
        public CommonPageViewModel<MemberInfoPageList> QueryMemberInfoPageList(MemberInfoPageQuery req)
        {
            var resData = _Rep.QueryMemberInfoPageList(req.AutoMapTo<QueryMemberInfoPageDto>());
            CommonPageViewModel<MemberInfoPageList> res = new CommonPageViewModel<MemberInfoPageList>();
            res = _Mapper.Map<CommonPageDto<List<MemberInfoListDto>>, CommonPageViewModel<MemberInfoPageList>>(resData);
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
                if (req.Password.Equals(req.MemberConfirmPassword)) 
                {
                    res.Msg = "两次密码不对应！";
                    return res;
                }
                var reqData = req.MapTo<AddMemberInfoDto>();
                reqData.CreateName = _accessor.HttpContext.User.Claims.SingleOrDefault(x => x.Type == "UserName").Value;
                var resData = _Rep.AddMemberInfo(reqData);
                res= _Mapper.Map<CommonViewModel>(resData);
            }
            else 
            {
                var reqData = req.MapTo<UpdateMemberInfoDto>();
                var resData = _Rep.UpdateMemberInfo(reqData);
                res = _Mapper.Map<CommonViewModel>(resData);
            }
            return res;
            
        }

       
    }
}
