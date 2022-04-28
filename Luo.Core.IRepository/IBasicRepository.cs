using Luo.Core.DatabaseFactory;
using Luo.Core.Models.Dtos.Request;
using Luo.Core.Models.Dtos.Response;

namespace Luo.Core.IRepository
{
    public interface IBasicRepository : ISqlSugarRepository
    {
        /// <summary>
        /// 添加初始化数据
        /// </summary>
        /// <returns></returns>
        public bool AddInitUser();
        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <returns></returns>
        public UserInfoDto QueryUserInfo(QueryUserInfoDto req);
    }
}