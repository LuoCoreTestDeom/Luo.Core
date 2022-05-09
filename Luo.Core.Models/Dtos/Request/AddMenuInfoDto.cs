using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luo.Core.Models.Dtos.Request
{
    public class AddMenuInfoDto
    {
        public string MenuName { get; set; }
        public string MenuAddress { get; set; }
        public string MenuIcon { get; set; }
        public int MenuSort { get; set; }

        public EnumModels.MenuTypeEnum MenuType { get; set; }

        public int ParentMenuId { get; set; }
        public bool MenuEnable { get; set; }

    }
}
