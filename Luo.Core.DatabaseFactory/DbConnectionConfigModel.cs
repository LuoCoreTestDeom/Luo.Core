namespace Luo.Core.DatabaseFactory;

public class DbConnectionConfigModel
{
    public string ConnectionString { get; set; }
    public SqlSugar.DbType DatabaseType { get; set; }
    public string ConfigId { get; set; }

    public class SlaveConnectionConfigModel
    {
        public string ConnectionString { get; set; }
        public int HitRate { get; set; }
    }

    public List<SlaveConnectionConfigModel> SlaveConnectionConfigs { get; set; }
}