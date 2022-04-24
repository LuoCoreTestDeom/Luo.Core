using Luo.Core.DatabaseFactory;
using Luo.Core.IRepository;
using Luo.Core.IServices;

namespace Luo.Core.Services
{
    public class DataInitService : SqlSugarRepositoryList<ISqlSugarFactory, IBasicRepository>, IServices.DataInitService
    {
        IBasicRepository _dataInitRepository;
        public DataInitService(ISqlSugarFactory factory, IBasicRepository databaseInit) : base(factory)
        {
            _dataInitRepository = databaseInit;
        }

        public bool InitUser() 
        {
           return _dataInitRepository.AddInitUser();
        }
    }
}