using Luo.Core.DatabaseFactory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luo.Core.Utility.ServiceExtensions
{
    public static class DatabaseInitSetup
    {
        public static void InitEntityData(this IServiceCollection services)
        {
            var initDatabase = services.BuildServiceProvider().GetService<ISqlSugarInitDatabase>();
            initDatabase.CreateDatabase();
            initDatabase.CreateDatabaseTables("Luo.Core.DatabaseEntity");
            string executDirPath = Directory.GetCurrentDirectory();
            executDirPath = executDirPath.Replace("Luo.Core.LayuiAdmin", "Luo.Core.DatabaseEntity");
            initDatabase.CreateDatabaseEntityFile(executDirPath, "Luo.Core.DatabaseEntity");
        }
    }
}
