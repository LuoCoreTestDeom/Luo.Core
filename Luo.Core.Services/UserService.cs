using AutoMapper;
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
        public readonly IMapper _mapper;
        public UserService(ISqlSugarFactory factory, IBasicRepository basicRepository, IMapper mapper) : base(factory)
        {
            _basicRepository = basicRepository;
            _mapper = mapper;
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
        public CommonViewModel<List<UserInfoDto>> UserLogin(UserLoginViewModel req)
        {
            CommonViewModel<List<UserInfoDto>> res = new CommonViewModel<List<UserInfoDto>>();
            var reqData= _mapper.Map<QueryUserInfoDto>(req);
            res.ResultData = _basicRepository.QueryUserInfo(reqData);
            return res;
        }
    }
}