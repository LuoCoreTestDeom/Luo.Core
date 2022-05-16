using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luo.Core.Utility.ServiceExtensions
{
    public static class SwaggerSetup
    {
        public static void AddSwaggerService<T>(this IServiceCollection services, string apiContactUrl = "", bool minimalState = true)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            //添加Swagger.
            services.AddSwaggerGen(sgo =>
            {
                var apiType = typeof(T);

                apiType.GetEnumNames().ToList().ForEach(version =>
                {

                    OpenApiInfo oai = new OpenApiInfo()
                    {
                        Title = apiType.Name + "·" + version,
                        Version = version,
                        Description = apiType.Name + "·" + version + "_API接口"
                    };
                    if (!string.IsNullOrWhiteSpace(apiContactUrl))
                    {
                        oai.Contact = new OpenApiContact
                        {
                            Name = apiType.Name + "·" + version + "_文档说明",
                            Email = string.Empty,
                            Url = new Uri(apiContactUrl)
                        };
                    }
                    sgo.SwaggerDoc(version, oai);
                });
                // 接口排序
                sgo.OrderActionsBy(o => o.GroupName + "," + o.HttpMethod + "," + o.RelativePath);
                //配置 xml 文档 创建一个DirectoryInfo的类
                DirectoryInfo directoryInfo = new DirectoryInfo(AppContext.BaseDirectory);
                //获取当前的目录的文件
                FileInfo[] fileInfos = directoryInfo.GetFiles("*.xml");
                foreach (FileInfo f in fileInfos)
                {
                    sgo.IncludeXmlComments(f.FullName, true);
                }
                if (!minimalState)
                {
                    #region 加锁
                    var openApiSecurity = new OpenApiSecurityScheme
                    {
                        Description = "JWT认证授权，使用直接在下框中输入Bearer {token}（注意两者之间是一个空格）\"",
                        Name = "Authorization",  //jwt 默认参数名称
                        In = ParameterLocation.Header,  //jwt默认存放Authorization信息的位置（请求头）
                        Type = SecuritySchemeType.ApiKey
                    };
                    sgo.AddSecurityDefinition("oauth2", openApiSecurity);
                    sgo.OperationFilter<AddResponseHeadersFilter>();
                    sgo.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                    sgo.OperationFilter<SecurityRequirementsOperationFilter>();
                    #endregion 开启加权小锁
                }
                //Show the api version in url address
                //sgo.DocInclusionPredicate((version, apiDescription) =>
                //{
                //    if (!version.Equals(apiDescription.GroupName))
                //        return false;

                //    var values = apiDescription.RelativePath
                //        .Split('/')
                //        .Select(v => v.Replace("v{version}", apiDescription.GroupName));

                //    apiDescription.RelativePath = string.Join("/", values);
                //    return true;
                //});
                //定义JwtBearer认证方式一
                sgo.AddSecurityDefinition("JwtBearer", new OpenApiSecurityScheme()
                {
                    Description = "这是方式一(直接在输入框中输入认证信息，不需要在开头添加Bearer)",
                    Name = "Authorization",//jwt默认的参数名称
                    In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置(请求头中)
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer"
                });
                //定义JwtBearer认证方式二
                //options.AddSecurityDefinition("JwtBearer", new OpenApiSecurityScheme()
                //{
                //    Description = "这是方式二(JWT授权(数据将在请求头中进行传输) 直接在下框中输入Bearer {token}（注意两者之间是一个空格）)",
                //    Name = "Authorization",//jwt默认的参数名称
                //    In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置(请求头中)
                //    Type = SecuritySchemeType.ApiKey
                //});
                //声明一个Scheme，注意下面的Id要和上面AddSecurityDefinition中的参数name一致
                var scheme = new OpenApiSecurityScheme()
                {
                    Reference = new OpenApiReference() { Type = ReferenceType.SecurityScheme, Id = "JwtBearer" }
                };
                //注册全局认证（所有的接口都可以使用认证）
                sgo.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    [scheme] = new string[0]
                });

            });
        }
        
        
    }
}
