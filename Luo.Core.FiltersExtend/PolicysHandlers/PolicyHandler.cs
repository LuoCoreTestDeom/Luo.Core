using Luo.Core.FiltersExtend.JsonWebToken;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
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
        public PolicyHandler(IAuthenticationSchemeProvider schemes, IHttpContextAccessor accessor)
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
                mvcContext.Result = new RedirectToActionResult("Login", "User", null);
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
                        AuthFail(context,httpContext);
                        return;
                    }
                    // 获取当前用户的角色信息
                    var currentPermissions = httpContext.User.Claims;
                    if (currentPermissions == null)
                    {
                        AuthFail(context, httpContext);
                        return;
                    }
                    #region 验证安全码
                    var claimSecurityKey = currentPermissions.SingleOrDefault(s => s.Type == "SecurityKey");
                    var singingSecurityKey = requirement.SigningCredentials.Key as SymmetricSecurityKey;
                    if (claimSecurityKey == null || singingSecurityKey == null)
                    {
                        AuthFail(context, httpContext);
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(claimSecurityKey.Value))
                    {
                        AuthFail(context, httpContext);
                        return;
                    }
                    var strClaimSecurityKey = Luo.Core.Common.SecurityEncryptDecrypt.CommonUtil.DecryptString(claimSecurityKey.Value);
                    var strSingingSecurityKey = Encoding.Default.GetString(singingSecurityKey.Key);
                    if (!strClaimSecurityKey.StartsWith(strSingingSecurityKey))
                    {
                        AuthFail(context, httpContext);
                        return;
                    }
                    #endregion 验证安全码
                    context.Succeed(requirement);
                    return;
                }

            }

        }

        private void AuthFail(AuthorizationHandlerContext context, HttpContext hc) 
        {
            context.Fail();
            string js = " <script language=javascript>top.location.href='{0}'</script> ";
            hc.Response.WriteAsync(string.Format(js, "/User/Login"));
        }



    }
}
