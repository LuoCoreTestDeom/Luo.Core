using Luo.Core.DatabaseFactory;

namespace Luo.Core.IRepository
{
    public interface IBasicRepository : ISqlSugarRepository
    {
        /// <summary>
        /// 添加初始化数据
        /// </summary>
        /// <returns></returns>
        public bool AddInitUser();
    }
}