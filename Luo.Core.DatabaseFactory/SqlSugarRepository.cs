using SqlSugar;

using System.Reflection;

namespace Luo.Core.DatabaseFactory;

//创建（Create、Add、Insert）、更新（Update、PUT、Write）、读取（Retrieve、SELECT、GET 、Read）  和删除（Delete、Dispose）操作。
public class SqlSugarRepository<TFactory, TIRepository> : ISqlSugarRepository
    where TFactory : ISqlSugarFactory
    where TIRepository : ISqlSugarRepository
{
    protected readonly ISqlSugarFactory Factory;
    protected SqlSugarClient DbContext => this.Factory.GetDbContext();

    public SqlSugarRepository(ISqlSugarFactory factory) => Factory = factory;

    public void CreateDatabaseEntityFile(string strPath, string classNameSpace)
    {
        DbContext.DbFirst.IsCreateAttribute().CreateClassFile(strPath, classNameSpace);
    }

    public void CreateDatabase()
    {
        //如果不存在创建数据库
        DbContext.DbMaintenance.CreateDatabase(); //个别数据库不支持
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
                db.CodeFirst.InitTables(itemType);
            }
        });
    }
}

public class SqlSugarRepository<TFactory> : ISqlSugarRepository
    where TFactory : ISqlSugarFactory
{
    protected readonly TFactory Factory;
    protected SqlSugarClient DbContext => this.Factory.GetDbContext();

    public SqlSugarRepository(TFactory factory) => Factory = factory;

    public void CreateDatabaseEntityFile(string strPath, string classNameSpace)
    {
        DbContext.DbFirst.IsCreateAttribute().CreateClassFile(strPath, classNameSpace);
    }

    public void CreateDatabase()
    {
        //如果不存在创建数据库
        DbContext.DbMaintenance.CreateDatabase(); //个别数据库不支持
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
                db.CodeFirst.InitTables(itemType);
            }
        });
    }
}