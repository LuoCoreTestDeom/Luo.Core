using Luo.Core.DatabaseFactory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luo.Core.Utility.ServiceExtensions
{
    /// <summary>
    /// SqlSugar 启动服务
    /// </summary>
    public static class SqlsugarSetup
    {
        public static void AddSqlSugarSetup(this IServiceCollection services) 
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            var sqlConnectString = Appsettings.GetValue(new string[] { "ConnectionStrings", "SqlServer" });
            if (string.IsNullOrWhiteSpace(sqlConnectString))
            {
                //多租户，多库
                var dbConfigDatas = Appsettings.GetObjectList<DbConnectionConfigModel>("DbConnectionConfig");
                services.AddSqlSugarClients<SqlSugarsFactory>((dbconfigs) =>
                {
                    foreach (var item in dbConfigDatas)
                    {
                        if (string.IsNullOrWhiteSpace(item.ConnectionString)) 
                        {
                            continue;
                        }
                        var dbSqlConfig = new ConnectionConfig()
                        {
                            ConnectionString = SpecialDbString(item.DatabaseType, item.ConnectionString) ,
                            DbType = item.DatabaseType,
                            ConfigId = item.ConfigId,
                            IsAutoCloseConnection = true,//开启自动释放模式和EF原理一样我就不多解释了
                            InitKeyType = InitKeyType.Attribute//从特性读取主键和自增列信息
                        };
                        if (item.SlaveConnectionConfigs.Count > 0)
                        {
                            dbSqlConfig.SlaveConnectionConfigs = new List<SlaveConnectionConfig>();
                            foreach (var item2 in item.SlaveConnectionConfigs)
                            {
                                if (string.IsNullOrWhiteSpace(item2.ConnectionString))
                                {
                                    continue; 
                                }
                                dbSqlConfig.SlaveConnectionConfigs.Add(new SlaveConnectionConfig()
                                {
                                    ConnectionString = SpecialDbString(item.DatabaseType, item2.ConnectionString),
                                    HitRate = item2.HitRate
                                });
                            }
                        }
                        dbconfigs.Add(dbSqlConfig);
                    }

                });
            }
            else
            {
                //单库
                services.AddSqlSugarClient<SqlSugarFactory>((dbconfig) =>
                {
                    dbconfig.ConnectionString = sqlConnectString;
                    dbconfig.DbType = DbType.SqlServer;
                    dbconfig.IsAutoCloseConnection = true;//开启自动释放模式和EF原理一样我就不多解释了
                    dbconfig.InitKeyType = InitKeyType.Attribute;//从特性读取主键和自增列信息
                });
            }
        }
        /// <summary>
        /// 定制Db字符串
        /// 目的是保证安全：优先从本地txt文件获取，若没有文件则从appsettings.json中获取
        /// </summary>
        /// <param name="mutiDBOperate"></param>
        /// <returns></returns>
        private static string SpecialDbString(SqlSugar.DbType dbType,string strConnect)
        {
            string sqlConnectString = string.Empty;
            switch (dbType) 
            {

                case DbType.Sqlite: sqlConnectString= $"DataSource=" + Path.Combine(Environment.CurrentDirectory, strConnect); 
                    break;
                case DbType.SqlServer:
                    sqlConnectString = DifDBConnOfSecurity(@"D:\my-file\dbCountPsw1_SqlserverConn.txt", strConnect);
                    break;
            }
            return sqlConnectString;
        }
        private static string DifDBConnOfSecurity(params string[] conn)
        {
            foreach (var item in conn)
            {
                try
                {
                    if (File.Exists(item))
                    {
                        return File.ReadAllText(item).Trim();
                    }
                }
                catch (System.Exception) { }
            }

            return conn[conn.Length - 1];
        }
        // <summary>
        /// SqlSugar上下文注入
        /// </summary>
        /// <typeparam name="TSugarContext">要注册的上下文的类型</typeparam>
        /// <param name="serviceCollection"></param>
        /// <param name="configAction"></param>
        /// <param name="lifetime">用于在容器中注册TSugarClient服务的生命周期</param>
        /// <returns></returns>
        internal static IServiceCollection AddSqlSugarClient<TSugarContext>(this IServiceCollection serviceCollection, Action<ConnectionConfig> configAction, ServiceLifetime lifetime = ServiceLifetime.Scoped)
             where TSugarContext : ISqlSugarFactory
        {

            var sdContext = new ServiceDescriptor(typeof(TSugarContext), typeof(TSugarContext), lifetime);
            serviceCollection.TryAdd(sdContext);

            var sdSqlSugarConfig = new ServiceDescriptor(typeof(ConnectionConfig), sp => configAction, lifetime);

            serviceCollection.TryAdd(sdSqlSugarConfig);
            return serviceCollection;
        }


        // <summary>
        /// SqlSugar上下文注入
        /// </summary>
        /// <typeparam name="TSugarContext">要注册的上下文的类型</typeparam>
        /// <param name="serviceCollection"></param>
        /// <param name="configAction"></param>
        /// <param name="lifetime">用于在容器中注册TSugarClient服务的生命周期</param>
        /// <returns></returns>
        internal static IServiceCollection AddSqlSugarClients<TSugarContext>(this IServiceCollection serviceCollection, Action<List<ConnectionConfig>> configAction, ServiceLifetime lifetime = ServiceLifetime.Scoped)
             where TSugarContext : ISqlSugarFactory
        {

            var sdContext = new ServiceDescriptor(typeof(TSugarContext), typeof(TSugarContext), lifetime);
            serviceCollection.TryAdd(sdContext);

            var sdSqlSugarConfig = new ServiceDescriptor(typeof(ConnectionConfig), sp => configAction, lifetime);

            serviceCollection.TryAdd(sdSqlSugarConfig);
            return serviceCollection;
        }
    }
}
