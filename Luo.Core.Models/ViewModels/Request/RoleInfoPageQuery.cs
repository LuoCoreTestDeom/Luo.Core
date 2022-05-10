using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luo.Core.Models.ViewModels.Request
{
    public class RoleInfoPageQuery
    {
        public int PageIndex { get; set; }
        public int PageCount { get; set; }
        public string RoleName { get; set; }
    }
}
