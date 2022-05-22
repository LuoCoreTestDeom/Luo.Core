using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luo.Core.Utility.ApplicationExtend
{
    public static class UseCorsExtend
    {
        public static void UseCorsMiddleware(this IApplicationBuilder app) 
        {
            app.UseCors();
            app.UseMiddleware<CorsMiddleware>();
        }
    }
}
