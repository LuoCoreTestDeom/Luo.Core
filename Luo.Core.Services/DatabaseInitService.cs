using Luo.Core.DatabaseFactory;
using Luo.Core.IRepository;
using Luo.Core.IServices;

namespace Luo.Core.Services
{
    public class DatabaseInitService : SqlSugarRepositoryList<ISqlSugarFactory, IDatabaseInitRepository>, IDatabaseInitService
    {
        public DatabaseInitService(ISqlSugarFactory factory, IDatabaseInitRepository databaseInit) : base(factory)
        {
            //databaseInit.CreateDatabase();
            //databaseInit.CreateDatabaseTables();
        }
    }
}