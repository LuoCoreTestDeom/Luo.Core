using Luo.Core.Common;
using Luo.Core.Utility.JsonWebToken;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration.UserSecrets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Luo.Core.FiltersExtend.PolicysHandlers
{


    /// <summary>
    /// 权限授权处理器
    /// </summary>
    public class JwtPolicyHandler : AuthorizationHandler<JwtAutoRequirement>
    {
        /// <summary>
        /// 验证方案提供对象
        /// </summary>
        public IAuthenticationSchemeProvider Schemes { get; set; }
        private readonly IHttpContextAccessor _accessor;
        readonly IJwtAppService _jwtApp;

        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="schemes"></param>
        /// <param name="roleModulePermissionServices"></param>
        /// <param name="accessor"></param>
        public JwtPolicyHandler(IAuthenticationSchemeProvider schemes, IHttpContextAccessor accessor,IJwtAppService jwtService)
        {
            _accessor = accessor;
            Schemes = schemes;
            _jwtApp = jwtService;
        }

        // 重写异步处理程序
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, JwtAutoRequirement requirement)
        {
          
            var httpContext = _accessor.HttpContext;

            // 获取系统中所有的角色和菜单的关系集合
            if (!requirement.Permissions.Any())
            {
               
                var list = new List<PermissionItem>();
              
                requirement.Permissions = list;
            }

            if (httpContext != null)
            {
                //获取授权方式
                var defaultAuthenticate = await Schemes.GetDefaultAuthenticateSchemeAsync();
                if (defaultAuthenticate != null)
                {
                    //验证签发的用户信息
                    var result = await httpContext.AuthenticateAsync(defaultAuthenticate.Name);
                    if (result.Succeeded)
                    {
                        //判断是否为已停用的 Token
                        if (!await _jwtApp.IsCurrentActiveTokenAsync())
                        {
                            context.Fail();
                            return;
                        }

                        httpContext.User = result.Principal;

                        //判断角色与 Url 是否对应
                        //
                        var url = httpContext.Request.Path.Value.ToLower();
                        var role = httpContext.User.Claims.Where(c => c.Type == ClaimTypes.Role).FirstOrDefault().Value;


                        var exptime = httpContext.User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Expiration).Value;
                        //判断是否过期
                        if (DateTime.Parse(exptime) >= DateTime.UtcNow)
                        {
                            context.Succeed(requirement);
                        }
                        else
                        {
                            context.Fail();
                        }
                        return;
                    }
                }
                context.Fail();
            }

            //context.Succeed(requirement);
        }
    }
}
