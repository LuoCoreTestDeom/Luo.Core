using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luo.Core.Models.Dtos.Response
{
    public class MenuInfoDto
    {
        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public string MenuIcon { get; set; }

        public string MenuAddress { get; set; }
        public int MenuType { get; set; }
        public bool MenuEnable { get; set; }
        public int MenuSort { get; set; }
        public int ParentMenuId { get; set; }

    }
}
