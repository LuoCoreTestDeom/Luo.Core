using Luo.Core.Models.ViewModels.Response;
using Luo.Core.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luo.Core.IServices
{
    public   interface IMemberService
    {
        /// <summary>
        /// 查询会员信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public CommonPageViewModel<MemberInfoPageList> QueryMemberInfoPageList(Models.ViewModels.Request.MemberInfoPageQuery req);
    }
}
