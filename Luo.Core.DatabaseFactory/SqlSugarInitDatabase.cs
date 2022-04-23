using SqlSugar;
using System.Reflection;

namespace Luo.Core.DatabaseFactory
{
    public interface ISqlSugarInitDatabase
    {
        void CreateDatabase();

        void CreateDatabaseEntityFile(string strPath, string classNameSpace);

        void CreateDatabaseTables(string assemblyName);
    }

    public class SqlSugarInitDatabase : SqlSugarRepositoryList<ISqlSugarFactory>, ISqlSugarInitDatabase
    {
        private readonly List<ConnectionConfig> configList;

        public SqlSugarInitDatabase(ISqlSugarFactory factory) : base(factory)
        {
            var factoryList = factory as SqlSugarFactoryList;
            configList = factoryList.GetAllConnectionCOnfig();
        }

        public void CreateDatabaseEntityFile(string strPath, string classNameSpace)
        {
            foreach (var item in configList)
            {
                DbContext.GetConnection(item.ConfigId).DbFirst.IsCreateAttribute().CreateClassFile(strPath, classNameSpace);
            }
        }

        public void CreateDatabase()
        {
            foreach (var item in configList)
            {
                //如果不存在创建数据库
                DbContext.GetConnection(item.ConfigId).DbMaintenance.CreateDatabase(); //个别数据库不支持
            }
        }

        public void CreateDatabaseTables(string assemblyName)
        {
            Factory.GetDbContext((db) =>
            {
                //排除程序程序集中的接口、私有类、抽象类
                Assembly assembly = Assembly.Load(assemblyName);
                var typeList = assembly.GetTypes().Where(t => !t.IsInterface && !t.IsSealed && !t.IsAbstract).ToList();
                //历遍程序集中的类
                foreach (var itemType in typeList)
                {
                    if (configList != null)
                    {
                        foreach (var item in configList)
                        {
                            db.GetConnection(item.ConfigId).CodeFirst.InitTables(itemType);
                        }
                    }
                }
            });
        }
    }
}