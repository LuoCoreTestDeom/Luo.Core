using Microsoft.AspNetCore.Builder;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luo.Core.Utility.ApplicationExtend
{
    public static class SwaggerMiddleware
    {
        /// <summary>
        /// 无法搞懂 作用
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseSwaggerAuthorized(this IApplicationBuilder app)
        {
            return app.UseMiddleware<SwaggerAuthMiddleware>();
        }
        public static void UseSwaggerMiddle<T>(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                var apiType = typeof(T);
                apiType.GetEnumNames().ToList().ForEach(version =>
                {
                    c.SwaggerEndpoint($"/swagger/{version}/swagger.json", version);
                    // c.RoutePrefix = "";//路径配置，设置为空，表示直接访问该文件，表示直接在根域名（localhost:9002）访问该文件
                    //这个时候去launchSettings.json中把"launchUrl": "swagger/index.html"去掉， 然后直接访问localhost:9002/index.html即可 
                });
            });

            //app.UseSwaggerUI();

        }
    }
}
