using Luo.Core.Common;
using Luo.Core.DatabaseFactory;
using Luo.Core.Utility.Authorization;
using Luo.Core.Utility.Authorization.JsonWebToken;
using Luo.Core.Utility.Authorization.JsonWebToken.Handlers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luo.Core.Utility.ServiceExtensions
{
    public static class JwtSetup
    {
        public static void AddJwtService(this IServiceCollection services)
        {
            services.AddTransient<IJwtAppService, JwtAppService>();
            var jwtToken = Appsettings.GetObject<TokenConfig>("JwtConfig");
            // 开启Bearer认证
            services.AddAuthentication(x =>
            {
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                //Token Validation Parameters
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    //获取或设置要使用的Microsoft.IdentityModel.Tokens.SecurityKey用于签名验证。
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtToken.SecurityKey)),
                    ValidateIssuer = true,//验证颁发者
                    //获取或设置一个System.String，它表示将使用的有效发行者检查代币的发行者。
                    ValidIssuer = jwtToken.Issuer,
                    ValidateAudience = true,//验证使用者
                    //获取或设置一个字符串，该字符串表示将用于检查的有效受众反对令牌的观众。
                    ValidAudience = jwtToken.Audience,
                    //缓冲过期时间，总的有效时间等于这个时间加上jwt的过期时间，如果不配置，默认是5分钟
                    ClockSkew = TimeSpan.FromMinutes(Convert.ToDouble(jwtToken.AccessExpiration)),
                    ValidateLifetime = true//验证使用时限
                };
                x.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        var jwtHandler = new JwtSecurityTokenHandler();
                        var token = context.Request.Headers["Authorization"].ObjToString().Replace("Bearer ", "");
                        if (token.IsNotEmptyOrNull() && jwtHandler.CanReadToken(token))
                        {
                            var jwtObj = jwtHandler.ReadJwtToken(token);

                            if (jwtObj.Issuer != jwtToken.Issuer)
                            {
                                context.Response.Headers.Add("Token-Error-Iss", "issuer is wrong!");
                            }
                            
                            if (jwtObj.Audiences.FirstOrDefault() != jwtToken.Audience)
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
