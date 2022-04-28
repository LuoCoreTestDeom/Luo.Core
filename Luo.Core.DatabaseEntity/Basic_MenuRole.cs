using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace Luo.Core.DatabaseEntity
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("Basic_MenuRole")]
    public partial class Basic_MenuRole
    {
           public Basic_MenuRole(){


           }
           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           [SugarColumn(IsPrimaryKey=true,IsIdentity=true)]
           public int Id {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int MenuId {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int RoleId {get;set;}

    }
}
