using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace Luo.Core.DatabaseEntity
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("Basic_Menu")]
    public partial class Basic_Menu
    {
        public Basic_Menu()
        {


        }
        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>           
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>       
        [SugarColumn(Length =50)]
        public string MenuName { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>     
        [SugarColumn(IsNullable = true)]
        public string? MenuAddress { get; set; }
        /// <summary>
        /// Desc: 0目录，1菜单，2按钮
        /// Default:
        /// Nullable:False
        /// </summary>           
        public int MenuType { get; set; }
        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>           
        public int ParentMenuId { get; set; }

        public bool MenuEnable { get; set; }
        public int MenuSort { get; set; }

        public string MenuIcon { get; set; }

    }
}
