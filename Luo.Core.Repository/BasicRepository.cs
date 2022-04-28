using Luo.Core.DatabaseEntity;
using Luo.Core.DatabaseFactory;
using Luo.Core.IRepository;
using Luo.Core.Models.Dtos.Request;
using Luo.Core.Models.Dtos.Response;
using Org.BouncyCastle.Asn1.X509;

namespace Luo.Core.Repository
{
    public class BasicRepository : SqlSugarRepositoryList<ISqlSugarFactory>, IBasicRepository
    {
        public BasicRepository(ISqlSugarFactory factory) : base(factory)
        {
        }
        /// <summary>
        /// 添加初始化数据
        /// </summary>
        /// <returns></returns>
        public bool AddInitUser()
        {
            bool res = false;
            string strValue = Luo.Core.Common.SecurityEncryptDecrypt.CommonUtil.EncryptString("123456");
            Factory.GetDbContext((db) =>
            {
                res = db.Insertable<Basic_User>(new
                {
                    UserName = "Luo",
                    Password = strValue,
                    CreateTime = DateTime.Now,
                    CreateName = "LuoCore"
                }).ExecuteCommand() > 0;
            });
            return res;
        }

        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <returns></returns>
        public List<UserInfoDto> QueryUserInfo(QueryUserInfoDto req)
        {
            List<UserInfoDto> res = new List<UserInfoDto>();
         
            Factory.GetDbContext((db) =>
            {
                res = db.Queryable<Basic_User>()
                .LeftJoin<Basic_UserRole>((a, b) => b.UserId == a.Id)
                .LeftJoin<Basic_Role>((a, b, c) => c.Id == b.RoleId)
                .Where((a, b, c) => a.UserName == req.UserName && a.Password == req.Password)
                .Select((a, b, c) => new UserInfoDto
                {
                    UserId = a.Id,
                    UserName = a.UserName,
                    RoleId = c.Id,
                    RoleName = c.RoleName
                }).ToList();
            });
            return res;
        }
    }
}