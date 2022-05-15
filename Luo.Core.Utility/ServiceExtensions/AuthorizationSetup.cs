using Google.Protobuf.WellKnownTypes;
using Luo.Core.Common;
using Luo.Core.FiltersExtend;
using Luo.Core.FiltersExtend.PolicysHandlers;
using Luo.Core.Utility.JsonWebToken;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Luo.Core.Utility.ServiceExtensions
{
    public static class AuthorizationSetup
    {
        public static void AddAuthCookieSetup(this IServiceCollection services)
        {
            // 以下四种常见的授权方式。

            // 1、这个很简单，其他什么都不用做， 只需要在API层的controller上边，增加特性即可
            // [Authorize(Roles = "Admin,System")]
            // 2、这个和上边的异曲同工，好处就是不用在controller中，写多个 roles 。
            // 然后这么写 [Authorize(Policy = "Admin")]
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Client", policy => policy.RequireRole("Client").Build());
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin").Build());
                options.AddPolicy("SystemOrAdmin", policy => policy.RequireRole("Admin", "System"));
                options.AddPolicy("A_S_O", policy => policy.RequireRole("Admin", "System", "Others"));
            });

          
            var permission = new List<PermissionItem>();
            // 角色与接口的权限要求参数
            var permissionRequirement = new PolicyRequirement(
                permission,
                ClaimTypes.Role,//基于角色的授权
        new SigningCredentials(new SymmetricSecurityKey(Encoding.Default.GetBytes("LuoCore")), SecurityAlgorithms.HmacSha256)
                );
            //"SecurityKey", CommonUtil.EncryptString("LuoCore" +ClaimTypes.Role)
            // 3、自定义复杂的策略授权
            services.AddAuthorization(options =>
{
                options.AddPolicy(GlobalVars.PermissionsName, policy => policy.Requirements.Add(permissionRequirement));
            });
            services.AddAuthCookieService();
            //注入权限处理器
            services.AddSingleton<IAuthorizationHandler, PolicyHandler>();
        }
        private static void AddAuthCookieService(this IServiceCollection services)
        {
            AuthenticationBuilder authBuilder = services.AddAuthentication(x =>
            {
                x.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                x.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                x.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                x.DefaultForbidScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                x.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            });
            authBuilder.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                //登入地址
                options.LoginPath = Appsettings.GetValue("AuthCookieConfig", "LoginPath");
                //登出地址
                options.LogoutPath = Appsettings.GetValue("AuthCookieConfig", "LoginPath");
                options.AccessDeniedPath = Appsettings.GetValue("AuthCookieConfig", "AccessDeniedPath");
            });
        }
        private static void AddAuthJwtService(this IServiceCollection services)
        {
            services.AddTransient<IJwtAppService, JwtAppService>();
           
          

            AuthenticationBuilder authBuilder = services.AddAuthentication(x =>
            {
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            });

            #region 参数


            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TokenConfig.JwtData.Secret)), SecurityAlgorithms.HmacSha256);

            // 如果要数据库动态绑定，这里先留个空，后边处理器里动态赋值
            var permission = new List<PermissionItem>();

            // 角色与接口的权限要求参数
            //var permissionRequirement = new PolicyRequirement(
            //    Appsettings.GetValue("AuthConfig", "LoginPath"),
            //     Appsettings.GetValue("AuthConfig", "AccessDeniedPath"),// 拒绝授权的跳转地址（目前无用）
            //    permission,
            //    ClaimTypes.NameIdentifier,//基于角色的授权
            //    jwtToken.Issuer,//发行人
            //    jwtToken.Audience,//听众
            //    signingCredentials,//签名凭据
            //    expiration: TimeSpan.FromSeconds(jwtToken.AccessExpiration)//接口的过期时间
            //    );
            PolicyRequirement permissionRequirement = new PolicyRequirement(null, ClaimTypes.NameIdentifier, signingCredentials);
            #endregion 参数
            services.AddAuthorization(options =>
            {
                options.AddPolicy(JwtBearerDefaults.AuthenticationScheme, policy => policy.Requirements.Add(permissionRequirement));
            });

            authBuilder.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, x =>
            {
                
                x.Audience = "www.luocore.com";
                x.Authority = "www.luocore.com";
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                //Token Validation Parameters
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    //获取或设置要使用的Microsoft.IdentityModel.Tokens.SecurityKey用于签名验证。
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TokenConfig.JwtData.Secret)),
                    ValidateIssuer = true,//验证颁发者
                    //获取或设置一个System.String，它表示将使用的有效发行者检查代币的发行者。
                    ValidIssuer = TokenConfig.JwtData.Issuer,
                    ValidateAudience = true,//验证使用者
                    //获取或设置一个字符串，该字符串表示将用于检查的有效受众反对令牌的观众。
                    ValidAudience = TokenConfig.JwtData.Audience,
                    //缓冲过期时间，总的有效时间等于这个时间加上jwt的过期时间，如果不配置，默认是5分钟
                    ClockSkew = TimeSpan.FromSeconds(Convert.ToDouble(TokenConfig.JwtData.ExpirationSeconds)),
                    ValidateLifetime = true//验证使用时限
                };
                x.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["access_token"];
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = context =>
                    {
                        var jwtHandler = new JwtSecurityTokenHandler();
                        var token = context.Request.Headers["Authorization"].ObjToString().Replace("Bearer ", "");
                        if (token.IsNotEmptyOrNull() && jwtHandler.CanReadToken(token))
                        {
                            var jwtObj = jwtHandler.ReadJwtToken(token);

                            if (jwtObj.Issuer != TokenConfig.JwtData.Issuer)
                            {
                                context.Response.Headers.Add("Token-Error-Iss", "issuer is wrong!");
                            }

                            if (jwtObj.Audiences.FirstOrDefault() != TokenConfig.JwtData.Audience)
                            {
                                context.Response.Headers.Add("Token-Error-Aud", "Audience is wrong!");
                            }
                        }
                        //Token expired 如果过期，则把<是否过期>添加到，返回头信息中
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });

        }
    }
}
