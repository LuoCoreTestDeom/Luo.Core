using SqlSugar;

namespace Luo.Core.DatabaseFactory
{
    public interface ISqlSugarFactory
    {
        SqlSugarClient GetDbContext(Action<string, SugarParameter[]> onExecutedEvent);

        SqlSugarClient GetDbContext(Func<string, SugarParameter[], KeyValuePair<string, SugarParameter[]>> onExecutingChangeSqlEvent);

        SqlSugarClient GetDbContext(Action<string, SugarParameter[]> onExecutedEvent = null, Func<string, SugarParameter[], KeyValuePair<string, SugarParameter[]>> onExecutingChangeSqlEvent = null, Action<Exception> onErrorEvent = null);

        void GetDbContext(Action<SqlSugarClient> Func);

        public T GetDbContext<T>(Func<SqlSugarClient, T> Func);
    }
}