using Luo.Core.Models.Dtos.Response;
using Luo.Core.Models.ViewModels;
using Luo.Core.Models.ViewModels.Request;

namespace Luo.Core.IServices
{
    public interface IUserService
    {
        public bool InitUser();
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public CommonViewModel<UserInfoDto> UserLogin(UserLoginViewModel req);
    }
}