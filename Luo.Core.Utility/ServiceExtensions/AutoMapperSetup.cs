using Luo.Core.Utility.AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luo.Core.Utility.ServiceExtensions
{
    /// <summary>
    /// Automapper 启动服务
    /// </summary>
    public static class AutoMapperSetup
    {
        public static void AddAutoMapperSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            //添加AutoMapper 扩展方法需要
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddAutoMapper(typeof(AutoMapperConfig));
            AutoMapperConfig.RegisterMappings();
        }
    }
}
