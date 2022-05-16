using AutoMapper;
using Luo.Core.DatabaseFactory;
using Luo.Core.IRepository;
using Luo.Core.Models.Dtos;
using Luo.Core.Models.Dtos.Request;
using Luo.Core.Models.Dtos.Response;
using Luo.Core.Models.ViewModels;
using Luo.Core.Models.ViewModels.Response;
using Luo.Core.Utility.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luo.Core.Services
{
    public class MemberService : SqlSugarRepositoryList<ISqlSugarFactory, IBasicRepository>, IServices.IMemberService
    {
        public MemberService(ISqlSugarFactory factory, IBasicRepository rep, IMapper mapper) : base(factory, rep, mapper)
        {
        }
        /// <summary>
        /// 查询会员信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public CommonPageViewModel<MemberInfoPageList> QueryMemberInfoPageList(Models.ViewModels.Request.MemberInfoPageQuery req) 
        {
           var resData=  _Rep.QueryMemberInfoPageList(req.AutoMapTo<QueryMemberInfoPageDto>());
            CommonPageViewModel<MemberInfoPageList> res = new CommonPageViewModel<MemberInfoPageList>();
            res = _Mapper.Map<CommonPageDto<List<MemberInfoListDto>>, CommonPageViewModel<MemberInfoPageList>>(resData);
            return res;
        }
    }
}
