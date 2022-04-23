using SqlSugar;

namespace Luo.Core.DatabaseFactory;

//创建（Create、Add、Insert）、更新（Update、PUT、Write）、读取（Retrieve、SELECT、GET 、Read）  和删除（Delete、Dispose）操作。
public class SqlSugarRepositoryList<TFactory, TIRepository> : ISqlSugarRepository
    where TFactory : ISqlSugarFactory
    where TIRepository : ISqlSugarRepository
{
    protected readonly TFactory Factory;
    protected SqlSugarClient DbContext => this.Factory.GetDbContext();

    public SqlSugarRepositoryList(TFactory factory) => Factory = factory;
}

public class SqlSugarRepositoryList<TFactory> : ISqlSugarRepository
    where TFactory : ISqlSugarFactory
{
    protected readonly TFactory Factory;
    protected SqlSugarClient DbContext => this.Factory.GetDbContext();

    public SqlSugarRepositoryList(TFactory factory) => Factory = factory;
}