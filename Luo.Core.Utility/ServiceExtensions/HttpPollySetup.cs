using Luo.Core.Common.HttpPolly;
using Luo.Core.EnumModels;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luo.Core.Utility.ServiceExtensions
{
    /// <summary>
    /// Polly 是一个 .NET 弹性和瞬态故障处理库
    /// </summary>
    public static class HttpPollySetup
    {
        public static void AddHttpPollySetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            #region Polly策略
            var retryPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .Or<TimeoutRejectedException>() // 若超时则抛出此异常
            .WaitAndRetryAsync(new[]
            {
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(5),
                TimeSpan.FromSeconds(10)
            });

            // 为每个重试定义超时策略
            var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(10);
            #endregion
            //添加NuGet包：Microsoft.Extensions.Http
            services.AddHttpClient(HttpEnum.Common.ToString(), c =>
            {
                c.DefaultRequestHeaders.Add("Accept", "application/json");
            })
            //添加NuGet包：Microsoft.Extensions.Http.Polly
            .AddPolicyHandler(retryPolicy)
            // 将超时策略放在重试策略之内，每次重试会应用此超时策略
            .AddPolicyHandler(timeoutPolicy);

            services.AddHttpClient(HttpEnum.LocalHost.ToString(), c =>
            {
                c.BaseAddress = new Uri("http://www.localhost.com");
                c.DefaultRequestHeaders.Add("Accept", "application/json");
            })
            .AddPolicyHandler(retryPolicy)
            // 将超时策略放在重试策略之内，每次重试会应用此超时策略
            .AddPolicyHandler(timeoutPolicy);

            services.AddSingleton<IHttpPollyHelper, HttpPollyHelper>();
        }
    }
}
