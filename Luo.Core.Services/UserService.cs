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
        IBasicRepository _basicRepository;

        public UserService(ISqlSugarFactory factory, IMapper mapper, IBasicRepository basicRepository) : base(factory, mapper)
        {
            _basicRepository = basicRepository;
        }

    

        public bool InitUser()
        {
            return _basicRepository.AddInitUser();
        }
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public CommonViewModel<LoginUserInfoDto> UserLogin(LoginUserViewModel req)
        {
            CommonViewModel<LoginUserInfoDto> res = new CommonViewModel<LoginUserInfoDto>();
            var reqData = _Mapper.Map<LoginUserDto>(req);
            try
            {
                reqData.Password = CommonUtil.EncryptString(reqData.Password);
                res.ResultData = _basicRepository.QueryUserInfo(reqData);
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