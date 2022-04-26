using Luo.Core.FiltersExtend.JsonWebToken;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Luo.Core.FiltersExtend.PolicysHandlers
{
    /// <summary>
    /// 角色策略授权处理
    /// 权限授权处理器
    /// </summary>
    public class PolicyHandler : AuthorizationHandler<PolicyRequirement>
    {
        /// <summary>
        /// 授权方式（cookie, bearer, oauth, openid）
        /// </summary>
        public IAuthenticationSchemeProvider Schemes { get; set; }
        private readonly IHttpContextAccessor _accessor;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="schemes"></param>
        /// <param name="jwtApp"></param>
        public PolicyHandler(IAuthenticationSchemeProvider schemes,  IHttpContextAccessor accessor)
        {
            Schemes = schemes;
            _accessor = accessor;
        }

        /// <summary>
        /// 授权处理
        ///  重写异步处理程序
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PolicyRequirement requirement)
        {
            if (requirement == null)
            {
                context.Fail();
                return;
            }
            if (context.Resource is AuthorizationFilterContext mvcContext)
            {
                mvcContext.Result = new RedirectToActionResult("Login","User",null);
            }

            AuthorizationFilterContext filterContext = context.Resource as AuthorizationFilterContext;

         
            
       

        //获取授权方式  判断请求是否拥有凭据，即有没有登录
        var defaultAuthenticate = await Schemes.GetDefaultAuthenticateSchemeAsync();
            if (defaultAuthenticate != null)
            {
                if (context.Resource is HttpContext httpContext)
                {
                    var endpoint = httpContext.GetEndpoint();
                    var actionDescriptor = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();
                    
                      //验证签发的用户信息
                      var result = await httpContext.AuthenticateAsync(defaultAuthenticate.Name);
                    if (!result.Succeeded)
                    {
                        context.Fail();
                        return;
                    }
                    // 获取当前用户的角色信息
                    var currentPermissions = httpContext.User.Claims;
                    if (currentPermissions == null)
                    {
                        context.Fail();
                        return;
                    }
                    var permissionsType = currentPermissions.SingleOrDefault(s => s.Type == JwtRegisteredClaimNames.Aud);
                    var userName = currentPermissions.SingleOrDefault(s => s.Type == requirement.ClaimType);
                    if (permissionsType == null || userName == null)
                    {
                        context.Fail();
                        return;
                    }
                    //验证权限
                    if (requirement.ClaimType != permissionsType.Value || !requirement.Permissions.Any(x => x.Role == userName.Value))
                    {
                        context.Fail();
                        return;
                    }
                    httpContext.User = result.Principal;

                    //判断角色与 Url 是否对应
                    //
                    var url = httpContext.Request.Path.Value.ToLower();
                    var role = httpContext.User.Claims.Where(c => c.Type == ClaimTypes.Role).FirstOrDefault().Value;


                    if (1 == null)
                    {
                        context.Fail();
                        return;
                    }

                    //判断是否过期
                    if (DateTime.Parse(httpContext.User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Expiration).Value) >= DateTime.UtcNow)
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

        }



    }
}
