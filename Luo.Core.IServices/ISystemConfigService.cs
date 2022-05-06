using Luo.Core.Models.ViewModels.Request;
using Luo.Core.Models.ViewModels.Response;
using Luo.Core.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luo.Core.Models.Dtos.Request;
using Luo.Core.Models.Dtos.Response;

namespace Luo.Core.IServices
{
    public interface ISystemConfigService
    {
        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public CommonViewModel<UserInfoList> QueryUserInfoList(UserInfoQuery req);

        /// <summary>
        /// 添加用户信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public CommonViewModel AddUser(UserInfoInput req);

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public CommonViewModel UpdateUser(UserInfoInput req);

        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public CommonViewModel DeleteUserByUserIds(List<int> userIds);
    }
}
