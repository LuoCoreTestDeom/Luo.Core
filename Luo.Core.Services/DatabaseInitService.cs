using Luo.Core.DatabaseFactory;
using Luo.Core.IRepository;
using Luo.Core.IServices;

namespace Luo.Core.Services
{
    public class DatabaseInitService : SqlSugarRepositoryList<ISqlSugarFactory, IBasicRepository>, IDatabaseInitService
    {
        public DatabaseInitService(ISqlSugarFactory factory, IBasicRepository databaseInit) : base(factory)
        {
            //databaseInit.CreateDatabase();
            //databaseInit.CreateDatabaseTables();
        }
    }
}