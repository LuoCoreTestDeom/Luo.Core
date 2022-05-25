using Luo.Core.Models.ViewModels.Response;
using Luo.Core.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luo.Core.Models.ViewModels.Request;

namespace Luo.Core.IServices
{
    public   interface IMemberService
    {
        /// <summary>
        /// 查询会员信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public CommonPageViewModel<List<MemberInfoResult>> QueryMemberInfoPageList(MemberInfoPageQuery req);
   
        /// <summary>
        /// 添加编辑会员信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public CommonViewModel AddEditMemberInfo(AddEditMemberInfoInput req);

        /// <summary>
        /// 查询会员信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public CommonViewModel<MemberInfoResult> LoginMemberInfo(MemberLogin req);
        /// <summary>
        /// 删除会员信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public CommonViewModel DeleteMemberByMemberId(List<int> req);
    }
}
