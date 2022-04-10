using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luo.Core.Utility
{
    /// <summary>
    /// appsettings.json操作类
    /// </summary>
    public class Appsettings
    {
        private static  IConfiguration _configuration;
        static string _contentPath { get; set; }
        public Appsettings(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public Appsettings(string contentPath)
        {
         
            string Path = "appsettings.json";

            //如果你把配置文件 是 根据环境变量来分开了，可以这样写
            //Path = $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json";

           _configuration = new ConfigurationBuilder()
               .SetBasePath(_contentPath)
               .Add(new JsonConfigurationSource { Path = Path, Optional = false, ReloadOnChange = true })//这样的话，可以直接读目录里的json文件，而不是 bin 文件夹下的，所以不用修改复制属性
               .Build();
        }




        /// <summary>
        /// 递归获取配置信息数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sections"></param>
        /// <returns></returns>
        public static T GetObject<T>(params string[] sections) where T:new()
        {
            T obj = new T();
            // 引用 Microsoft.Extensions.Configuration.Binder 包
            _configuration.Bind(string.Join(":", sections), obj);
            return obj;
        }
        /// <summary>
        /// 递归获取配置信息数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sections"></param>
        /// <returns></returns>
        public static List<T> GetObjectList<T>(params string[] sections)
        {
            List<T> list = new List<T>();
            // 引用 Microsoft.Extensions.Configuration.Binder 包
            _configuration.Bind(string.Join(":", sections), list);
            return list;
        }

        /// <summary>
        /// 封装要操作的字符
        /// </summary>
        /// <param name="sections">节点配置</param>
        /// <returns></returns>
        public static string GetValue(params string[] sections)
        {
            try
            {
                if (sections.Any())
                {
                    return _configuration[string.Join(":", sections)];
                }
            }
            catch (Exception) { }

            return "";
        }
        /// <summary>
        /// 根据路径  configuration["App:Name"];
        /// </summary>
        /// <param name="sectionsPath"></param>
        /// <returns></returns>
        public static string GetValue(string sectionsPath)
        {
            try
            {
                return _configuration[sectionsPath];
            }
            catch (Exception) { }

            return "";

        }
    }
}
