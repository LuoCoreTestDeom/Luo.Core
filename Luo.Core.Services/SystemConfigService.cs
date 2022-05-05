using Luo.Core.DatabaseFactory;
using Luo.Core.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luo.Core.Models.ViewModels.Request;
using Luo.Core.Models.ViewModels.Response;
using Luo.Core.Models.ViewModels;
using AutoMapper;

namespace Luo.Core.Services
{
    public class SystemConfigService : SqlSugarRepositoryList<ISqlSugarFactory, IBasicRepository>, IServices.ISystemConfigService
    {
        private readonly IBasicRepository _basicRepository;

        public SystemConfigService(ISqlSugarFactory factory, IMapper mapper, IBasicRepository basicRepository) : base(factory, mapper)
        {
            _basicRepository= basicRepository;
        }
        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public CommonViewModel<UserInfoListViewModel> QueryUserInfoList(QueryUserInfoViewModel req) 
        {
            CommonViewModel<UserInfoListViewModel> res = new CommonViewModel<UserInfoListViewModel>();
            try
            {
                var userList = _basicRepository.QueryUserInfoList(_Mapper.Map<Models.Dtos.Request.QueryUserInfoDto>(req));
                res.Status = true;
                res.StatusCode = 200;
                res.ResultData = _Mapper.Map<UserInfoListViewModel>(userList);
            }
            catch (Exception ex)
            {
                res.Msg = ex.Message;
            }
            return res;
        }
    }
}
