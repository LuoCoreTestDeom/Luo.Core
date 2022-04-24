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
        public void AddUser()
{
            Factory.GetDbContext((db) =>
            {
                db.Insertable(new Basic_User() 
                { 
                   UserName="Luo",
                   Password=""
                }).ExecuteCommand();
            }); 
        }
    }
}