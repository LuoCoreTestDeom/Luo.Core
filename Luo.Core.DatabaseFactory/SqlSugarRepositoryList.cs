using AutoMapper;
using Org.BouncyCastle.Crypto;
using SqlSugar;

namespace Luo.Core.DatabaseFactory;

//创建（Create、Add、Insert）、更新（Update、PUT、Write）、读取（Retrieve、SELECT、GET 、Read）  和删除（Delete、Dispose）操作。
public class SqlSugarRepositoryList<TFactory, TIRepository> : ISqlSugarRepository
    where TFactory : ISqlSugarFactory
    where TIRepository : ISqlSugarRepository
{
    protected readonly TFactory Factory;
    protected readonly IMapper _Mapper;
    protected readonly TIRepository _Rep;
    protected SqlSugarClient DbContext => this.Factory.GetDbContext();

    public SqlSugarRepositoryList(TFactory factory, TIRepository rep, IMapper mapper) 
    {
        Factory = factory;
        _Mapper = mapper;
        _Rep = rep;
    }
}

public class SqlSugarRepositoryList<TFactory> : ISqlSugarRepository
    where TFactory : ISqlSugarFactory
{
    protected readonly TFactory Factory;
    protected readonly IMapper _Mapper;
    protected SqlSugarClient DbContext => this.Factory.GetDbContext();

    public SqlSugarRepositoryList(TFactory factory, IMapper mapper)
    {
        Factory = factory;
        _Mapper = mapper;
    }
}