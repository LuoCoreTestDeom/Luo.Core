using SqlSugar;
namespace Luo.Core.DatabaseFactory;

public interface ISqlSugarRepository
{
}

//创建（Create、Add、Insert）、更新（Update、PUT、Write）、读取（Retrieve、SELECT、GET 、Read）  和删除（Delete、Dispose）操作。
public class SqlSugarRepository : ISqlSugarRepository
{
    protected readonly ISqlSugarFactory _factory;
    protected SqlSugarClient DbContext => this._factory.GetDbContext();
    public SqlSugarRepository(ISqlSugarFactory factory) => _factory = factory;
}
