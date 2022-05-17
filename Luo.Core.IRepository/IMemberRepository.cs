using Luo.Core.Models.Dtos.Request;
using Luo.Core.Models.Dtos.Response;
using Luo.Core.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luo.Core.DatabaseFactory;

namespace Luo.Core.IRepository
{
    public interface IMemberRepository : ISqlSugarRepository
    {
        /// <summary>
        /// 添加会员信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        CommonDto AddMemberInfo(AddMemberInfoDto req);
        /// <summary>
        /// 更新会员信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public CommonDto UpdateMemberInfo(UpdateMemberInfoDto req);
        /// <summary>
        /// 获取JWT登录会员信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        JwtLoginMemberInfoDto QueryJwtMemberInfo(JwtQueryMemberInfoDto req);
        /// <summary>
        /// 查询会员信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        CommonPageDto<List<MemberInfoListDto>> QueryMemberInfoPageList(QueryMemberInfoPageDto req);

        
    }
}
