using Luo.Core.Common.HttpPolly;
using Luo.Core.EnumModels;
using Microsoft.Extensions.DependencyInjection;
using Polly.Extensions.Http;
using Polly.Timeout;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luo.Core.Utility.ServiceExtensions
{
    public static class HttpRequestSetup
    {
        public static void AddHttpService(this IServiceCollection services,string url)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            services.AddHttpClient();
        }
    }
}
