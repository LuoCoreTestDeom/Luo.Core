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
                db.Deleteable<Basic_User>().Where(x => x.UserName == "Luo").ExecuteCommand();
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
        public UserInfoDto QueryUserInfo(QueryUserInfoDto req)
        {
            UserInfoDto res = new UserInfoDto();
         
            Factory.GetDbContext((db) =>
            {
                var result = db.Queryable<Basic_User>()
                .LeftJoin<Basic_UserRole>((a, b) => b.UserId == a.Id)
                .LeftJoin<Basic_Role>((a, b, c) => c.Id == b.RoleId)
                .Where((a, b, c) => a.UserName == req.UserName && a.Password == req.Password)
                .Select((a, b, c) => new 
                {
                    UserId = a.Id,
                    UserName = a.UserName,
                    RoleId = c.Id,
                    RoleName = c.RoleName
                }).ToList();
                if (result != null&& result.Count>0)
                {
                    res.UserId = result[0].UserId;
                    res.UserName=result[0].UserName;
                    res.RoleInfos = new List<RoleInfoDto>();
                    foreach (var item in result)
                    {
                        if (item.RoleId > 0) 
                        {
                            res.RoleInfos.Add(new RoleInfoDto()
                            {
                                RoleId = item.RoleId,
                                RoleName = item.RoleName
                            });
                        }
                        
                    }
                }
               
            });
            return res;
        }
    }
}