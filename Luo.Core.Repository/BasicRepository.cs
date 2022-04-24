using Luo.Core.DatabaseEntity;
using Luo.Core.DatabaseFactory;
using Luo.Core.IRepository;
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
            string strValue2 = Luo.Core.Common.SecurityEncryptDecrypt.CommonUtil.DecryptString(strValue,false);
            Factory.GetDbContext((db) =>
            {
                res= db.Insertable<Basic_User>(new
                {
                    UserName = "Luo",
                    Password = strValue,
                    CreateTime = DateTime.Now,
                    CreateName = "LuoCore"
                }).ExecuteCommand()>0;
            });
            return res;
        }
    }
}