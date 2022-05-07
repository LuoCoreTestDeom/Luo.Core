using AutoMapper;
using Luo.Core.Common.SecurityEncryptDecrypt;
using Luo.Core.DatabaseFactory;
using Luo.Core.IRepository;
using Luo.Core.IServices;
using Luo.Core.Models.Dtos.Request;
using Luo.Core.Models.Dtos.Response;
using Luo.Core.Models.ViewModels;
using Luo.Core.Models.ViewModels.Request;

namespace Luo.Core.Services
{
    public class UserService : SqlSugarRepositoryList<ISqlSugarFactory, IBasicRepository>, IServices.IUserService
    {
        public UserService(ISqlSugarFactory factory, IBasicRepository rep, IMapper mapper) : base(factory, rep, mapper)
        {
        }





        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public CommonViewModel<LoginUserInfoDto> UserLogin(LoginUserForm req)
        {
            CommonViewModel<LoginUserInfoDto> res = new CommonViewModel<LoginUserInfoDto>();
            var reqData = _Mapper.Map<LoginUserDto>(req);
            try
            {
                reqData.Password = CommonUtil.EncryptString(reqData.Password);
                res.ResultData = _Rep.QueryUserInfo(reqData);
                res.Status = true;
            }
            catch (Exception ex)
            {
                res.Msg = ex.Message;
            }

            return res;
        }

    
    }
}