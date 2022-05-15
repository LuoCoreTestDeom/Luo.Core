using Luo.Core.Common;
using Luo.Core.FiltersExtend.AuthorizationFilters;
using Luo.Core.FiltersExtend.PolicysHandlers;
using Luo.Core.Utility.JsonWebToken;
using Microsoft.AspNetCore.Authentication;
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
        public static void AddJWTService(this IServiceCollection services)
        {

            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            var jwtKeySecret = TokenConfig.JwtData?.Secret;
            if (!string.IsNullOrWhiteSpace(jwtKeySecret))
            {
                var keyByteArray = Encoding.UTF8.GetBytes(jwtKeySecret);
                var signingKey = new SymmetricSecurityKey(keyByteArray);
                var Issuer = TokenConfig.JwtData.Issuer;
                var Audience = TokenConfig.JwtData.Audience;
                var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);



                // 开启Bearer认证
                services.AddAuthentication(o =>
                {
                    o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    o.DefaultChallengeScheme = nameof(JwtAuthHandler);
                    o.DefaultForbidScheme = nameof(JwtAuthHandler);
                }) // 添加JwtBearer服务
                .AddJwtBearer(o =>
                {
                    // 令牌验证参数
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,//是否验证密钥
                        IssuerSigningKey = signingKey,//指定密钥
                        ValidateIssuer = true,//是否验证颁发者
                        ValidIssuer = Issuer,//证颁发者
                        ValidateAudience = true,//是否验证接受者
                        ValidAudience = Audience,//接受者
                        ValidateLifetime = true,//设置必须验证超时
                        RequireExpirationTime = true,//设置必须要有超时时间
                        ClockSkew = TimeSpan.FromSeconds(30)//将其赋值为0时，即设置有效时间到期，就马上失效
                    };
                    //认证的时候可以添加事件
                    o.Events = new JwtBearerEvents
                    {
                        OnChallenge = context =>
                        {
                            context.Response.Headers.Add("Token-Error", context.ErrorDescription);
                            return Task.CompletedTask;
                        },
                        OnAuthenticationFailed = context =>
                        {
                            var jwtHandler = new JwtSecurityTokenHandler();
                            var token = context.Request.Headers["Authorization"].ObjToString().Replace("Bearer ", "");

                            if (token.IsNotEmptyOrNull() && jwtHandler.CanReadToken(token))
                            {
                                var jwtToken = jwtHandler.ReadJwtToken(token);

                                if (jwtToken.Issuer != Issuer)
                                {
                                    context.Response.Headers.Add("Token-Error-Iss", "issuer is wrong!");
                                }

                                if (jwtToken.Audiences.FirstOrDefault() != Audience)
                                {
                                    context.Response.Headers.Add("Token-Error-Aud", "Audience is wrong!");
                                }
                            }
                            // 如果过期，则把<是否过期>添加到，返回头信息中
                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            {
                                context.Response.Headers.Add("Token-Expired", "true");
                            }
                            return Task.CompletedTask;
                        }
                    };
                })
                .AddScheme<AuthenticationSchemeOptions, JwtAuthHandler>(nameof(JwtAuthHandler), o =>
                {

                });

                services.AddScoped<IJwtAppService, JwtAppService>();
            }

        }


       
        
    }
}
