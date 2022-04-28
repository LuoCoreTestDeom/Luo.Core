using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace Luo.Core.DatabaseEntity
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("Basic_Role")]
    public partial class Basic_Role
    {
           public Basic_Role(){


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
           public string RoleName {get;set;}

    }
}
