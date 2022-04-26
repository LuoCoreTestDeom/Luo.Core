using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luo.Core.Utility.Authorization.JsonWebToken.Secret
{
    /// <summary>
    /// 用户或角色或其他凭据实体
    /// </summary>
    public class PermissionItem
    {
        public virtual Guid Id { get => Guid.NewGuid(); }
        /// <summary>
        /// 用户或角色或其他凭据名称
        /// </summary>
        public virtual string Role { get; set; }
        /// <summary>
        /// 请求Url
        /// </summary>
        public virtual string Url { get; set; }
    }
}
