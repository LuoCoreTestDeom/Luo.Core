using Luo.Core.Common;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luo.Core.Utility.ServiceExtensions
{
    /// <summary>
    /// Cors 启动服务
    /// </summary>
    public static class CorsSetup
    {
        public static void AddCorsSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            //需要添加 Microsoft.AspNetCore.Cors
            services.AddCors(c =>
            {
                if (!Appsettings.GetValue(new string[] { "Startup", "Cors", "EnableAllIPs" }).ObjToBool())
                {
                    c.AddPolicy(Appsettings.GetValue(new string[] { "Startup", "Cors", "PolicyName" }),

                        policy =>
                        {

                            policy
                            .WithOrigins(Appsettings.GetValue(new string[] { "Startup", "Cors", "IPs" }).Split(','))
                            .AllowAnyHeader()//Ensures that the policy allows any header. 确保策略允许任何报头。
                            .AllowAnyMethod();
                        });
                }
                else
                {
                    //允许任意跨域请求
                    c.AddPolicy(Appsettings.GetValue(new string[] { "Startup", "Cors", "PolicyName" }),
                        policy =>
                        {
                            policy
                            .SetIsOriginAllowed((host) => true)
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();
                        });
                }

            });
        }
    }
}
