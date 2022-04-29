using Luo.Core.Models.ViewModels.Request;
using Luo.Core.Models.ViewModels.Response;
using Luo.Core.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luo.Core.IServices
{
    public interface ISystemConfigService
    {
        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public CommonViewModel<UserInfoListViewModel> QueryUserInfoList(QueryUserInfoViewModel req);
    }
}
