using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luo.Core.Models.ViewModels.Response
{
    public class TreeRoleMenuResult
    {
        /// <summary>
        /// 节点标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 节点唯一索引值，用于对指定节点进行各类操作
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 节点字段名 一般对应表字段名
        /// </summary>
        public int Field { get; set; }
        public List<TreeRoleMenuResult> Children { get; set; }
        /// <summary>
        /// 节点是否初始展开，默认 false
        /// </summary>
        public bool Spread { get; set; } = false;
        public bool @Checked { get; set; }
        /// <summary>
        /// 节点是否为禁用状态。默认 false
        /// </summary>
        public bool Disabled { get; set; }
    }
}
