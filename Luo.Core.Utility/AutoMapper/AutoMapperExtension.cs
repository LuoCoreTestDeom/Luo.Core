using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luo.Core.Utility.AutoMapper
{

    ///summary 
    ///AutoMapper扩展方法 
    ///summary 
    public static class AutoMapperExtension
    {
        #region 袁俊飞
        /// <summary>
        ///  类型映射
        /// </summary>
        public static TDestination MapTo<TDestination>(this object source)
        {
            if (source == null) return default(TDestination);

            var mapper = new MapperConfiguration(a => a.CreateMap(source.GetType(), typeof(TDestination))).CreateMapper();
            return mapper.Map<TDestination>(source);
        }

        /// <summary>
        /// 集合列表类型映射
        /// </summary>
        public static List<TDestination> MapToList<TDestination>(this IEnumerable source)
        {
            IMapper mapper = null;

            foreach (var first in source)
            {
                mapper = new MapperConfiguration(a => a.CreateMap(first.GetType(), typeof(TDestination))).CreateMapper();
                break;
            }
            if (mapper == null)
            {
                return new List<TDestination>();
            }

            return mapper.Map<List<TDestination>>(source);
        }

        /// <summary>
        /// 集合列表类型映射
        /// </summary>
        public static List<TDestination> MapToList<TSource, TDestination>(this IEnumerable<TSource> source)
        {
            var mapper = new MapperConfiguration(a => a.CreateMap(typeof(TSource), typeof(TDestination))).CreateMapper();
            return mapper.Map<List<TDestination>>(source);
        }

        /// <summary>
        /// 类型映射
        /// </summary>
        public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination)
            where TSource : class
            where TDestination : class
        {
            if (source == null) return destination;

            var mapper = new MapperConfiguration(a => a.CreateMap(typeof(TSource), typeof(TDestination))).CreateMapper();
            return mapper.Map(source, destination);
        }

        /// <summary>
        /// DataReader映射
        /// </summary>
        public static IEnumerable<T> DataReaderMapTo<T>(this IDataReader reader)
        {
            var mapper = new MapperConfiguration(a => a.CreateMap(typeof(IDataReader), typeof(IEnumerable<T>))).CreateMapper();
            return mapper.Map<IDataReader, IEnumerable<T>>(reader);
        }
        /// <summary>
        ///  类型映射字段
        /// </summary>
        public static Dictionary<string, object> MapToDic(this object source)
        {
            if (source == null) return default(Dictionary<string, object>);
            var mapper = new MapperConfiguration(a => a.CreateMap(source.GetType(), typeof(Dictionary<string, object>)).ConstructUsing(b =>
                JsonConvert.DeserializeObject<Dictionary<string, object>>(JsonConvert.SerializeObject(source))
            )).CreateMapper();
            return mapper.Map<Dictionary<string, object>>(source);
        }
        #endregion 袁俊飞

        #region 博客园 www.cnblogs.com/mq0036/p/10670202.html
        /// <summary>
        ///  类型映射,默认字段名字一一对应
        /// </summary>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static TDestination AutoMapTo<TDestination>(this object obj)
        {
            if (obj == null) return default(TDestination);
            var config = new MapperConfiguration(cfg => cfg.CreateMap(obj.GetType(), typeof(TDestination)));
            return config.CreateMapper().Map<TDestination>(obj);
        }

   

        /// <summary>
        /// 类型映射,可指定映射字段的配置信息
        /// </summary>
        /// <typeparam name="TSource">源数据：要被转化的实体对象</typeparam>
        /// <typeparam name="TDestination">目标数据：转换后的实体对象</typeparam>
        /// <param name="source">任何引用类型对象</param>
        /// <param name="cfgExp">可为null，则自动一一映射</param>
        /// <returns></returns>
        public static TDestination AutoMapTo<TSource, TDestination>(this TSource source, Action<IMapperConfigurationExpression> cfgExp)
         where TDestination : class
         where TSource : class
        {
            if (source == null) return default(TDestination);
            var config = new MapperConfiguration(cfgExp != null ? cfgExp : cfg => cfg.CreateMap<TSource, TDestination>());
            var mapper = config.CreateMapper();
            return mapper.Map<TDestination>(source);
        }
        /// <summary>
        /// 类型映射,可指定映射字段的配置信息
        /// </summary>
        /// <typeparam name="TSource">源数据：要被转化的实体对象</typeparam>
        /// <typeparam name="TDestination">目标数据：转换后的实体对象</typeparam>
        /// <param name="source">任何引用类型对象</param>
        /// <param name="cfgExp">可为null，则自动一一映射</param>
        /// <returns></returns>
        public static TDestination AutoMapForMemberTo<TSource, TDestination>(this TSource source, Action<IMappingExpression<TSource, TDestination>> cfgExp)
         where TDestination : class
         where TSource : class
        {
            if (source == null|| cfgExp==null) return default(TDestination);
         
            var config = new MapperConfiguration(cfg => cfgExp(cfg.CreateMap<TSource, TDestination>()));
            var mapper = config.CreateMapper();
            return mapper.Map<TDestination>(source);
        }

       
        /// <summary>
        /// 类型映射,默认字段名字一一对应
        /// </summary>
        /// <typeparam name="TSource">源数据：要被转化的实体对象</typeparam>
        /// <typeparam name="TDestination">目标数据：转换后的实体对象</typeparam>
        /// <param name="source">任何引用类型对象</param>
        /// <returns>转化之后的实体</returns>
        public static TDestination AutoMapTo<TSource, TDestination>(this TSource source)
            where TDestination : class
            where TSource : class
        {
            if (source == null) return default(TDestination);
            var config = new MapperConfiguration(cfg => cfg.CreateMap<TSource, TDestination>());
            var mapper = config.CreateMapper();
            return mapper.Map<TDestination>(source);
        }
        /// <summary>
        /// 集合列表类型映射,默认字段名字一一对应
        /// </summary>
        /// <typeparam name="TDestination">转化之后的实体对象，可以理解为viewmodel</typeparam>
        /// <typeparam name="TSource">要被转化的实体对象，Entity</typeparam>
        /// <param name="source">通过泛型指定的这个扩展方法的类型，理论任何引用类型</param>
        /// <returns>转化之后的实体列表</returns>
        public static IEnumerable<TDestination> AutoMapTo<TSource, TDestination>(this IEnumerable<TSource> source)
            where TDestination : class
            where TSource : class
        {
            if (source == null) return new List<TDestination>();
            var config = new MapperConfiguration(cfg => cfg.CreateMap<TSource, TDestination>());
            var mapper = config.CreateMapper();
            return mapper.Map<List<TDestination>>(source);
        }
        #endregion 博客园 www.cnblogs.com/mq0036/p/10670202.html
    }
}
