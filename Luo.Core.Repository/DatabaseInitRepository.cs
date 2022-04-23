using Luo.Core.DatabaseFactory;
using Luo.Core.IRepository;

namespace Luo.Core.Repository
{
    public class DatabaseInitRepository : SqlSugarRepositoryList<ISqlSugarFactory>, IDatabaseInitRepository
    {
        public DatabaseInitRepository(ISqlSugarFactory factory) : base(factory)
        {
        }
    }
}