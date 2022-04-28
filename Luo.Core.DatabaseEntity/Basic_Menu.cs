﻿using System;
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
           public Basic_Menu(){


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
           public string MenuName {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string MenuAddress {get;set;}

    }
}
