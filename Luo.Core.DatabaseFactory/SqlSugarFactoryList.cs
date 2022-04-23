using SqlSugar;
using System.Diagnostics;

namespace Luo.Core.DatabaseFactory
{
    public class SqlSugarFactoryList : ISqlSugarFactory
    {
        public readonly List<ConnectionConfig> configs;

        public SqlSugarFactoryList(List<ConnectionConfig> _configs)
        {
            this.configs = _configs;
        }

        public List<ConnectionConfig> GetAllConnectionCOnfig()
        {
            return configs;
        }

        public SqlSugarClient GetDbContext(Action<string, SugarParameter[]> onExecutedEvent)
        {
            return GetDbContext(onExecutedEvent);
        }

        public SqlSugarClient GetDbContext(Func<string, SugarParameter[], KeyValuePair<string, SugarParameter[]>> onExecutingChangeSqlEvent)
        {
            return GetDbContext(null, onExecutingChangeSqlEvent);
        }

        public SqlSugarClient GetDbContext(Action<string, SugarParameter[]> onExecutedEvent = null, Func<string, SugarParameter[], KeyValuePair<string, SugarParameter[]>> onExecutingChangeSqlEvent = null, Action<Exception> onErrorEvent = null)
        {
            SqlSugarClient db = new SqlSugarClient(configs);
            SqlLog(db);
            return db;
        }

        public void GetDbContext(Action<SqlSugarClient> Func)
        {
            using (SqlSugarClient db = new SqlSugarClient(this.configs))
            {
                try
                {
                    SqlLog(db);
                    Func(db);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("SQL报错:" + ex);
                }
            }
        }

        public T GetDbContext<T>(Func<SqlSugarClient, T> Func)
        {
            using (SqlSugarClient db = new SqlSugarClient(this.configs))
            {
                try
                {
                    SqlLog(db);
                    return Func(db);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("SQL报错:" + ex);
                    return default(T);
                }
            }
        }

        internal void SqlLog(SqlSugarClient db)
        {
            foreach (var item in this.configs)
            {
                SqlSugarProvider sqlDb = db.GetConnection(item.ConfigId);
                sqlDb.Ado.IsEnableLogEvent = true;
                sqlDb.Aop.OnLogExecuting = (sql, pars) =>//每次Sql执行前事件
                {
                    Debug.WriteLine("Sql执行前:" + sql);
                    //我可以在这里面写逻辑
                };

                sqlDb.Aop.OnExecutingChangeSql = (sql, pars) => //可以修改SQL和参数的值
                {
                    Debug.WriteLine("修改SQL和参数的值:" + sql);
                    return new KeyValuePair<string, SugarParameter[]>(sql, pars);
                };
                sqlDb.Aop.OnLogExecuted = (sql, pars) => //SQL执行完
                {
                    Debug.Write("time:" + sqlDb.Ado.SqlExecutionTime.ToString());//输出SQL执行时间

                    if (sqlDb.Ado.SqlExecutionTime.TotalSeconds > 1)//执行时间超过1秒
                    {
                        //代码CS文件名
                        var fileName = sqlDb.Ado.SqlStackTrace.FirstFileName;
                        //代码行数
                        var fileLine = sqlDb.Ado.SqlStackTrace.FirstLine;
                        //方法名
                        var FirstMethodName = sqlDb.Ado.SqlStackTrace.FirstMethodName;
                        //db.Ado.SqlStackTrace.MyStackTraceList[1].xxx 获取上层方法的信息
                        Debug.WriteLine("SQL执行完,执行时间超过1秒:" + "【代码CS文件名：" + fileName + "】" + "【代码行数：" + fileLine + "】" + "【方法名：" + FirstMethodName + "】,获取上层方法的信息:" + string.Join(",", sqlDb.Ado.SqlStackTrace.MyStackTraceList));
                    }
                    Debug.WriteLine("SQL执行完:" + sql + "参数：" + string.Join(",", pars.Select(s => string.Format("{0}={1}", s.ParameterName, s.Value)).ToList()));
                };
                sqlDb.Aop.OnError = (exp) =>//SQL报错
                {
                    var dd = exp.Sql; //这样可以拿到错误SQL
                    Debug.WriteLine("ExecSql:" + exp.Sql);
                };
            }
        }
    }
}