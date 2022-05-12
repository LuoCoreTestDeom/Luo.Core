using Luo.Core.Models.ViewModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luo.Core.Models.ViewModels.Request
{
    public class AddEditRoleInfoInput
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public List<TreeRoleMenuResult> RoleMenuInfos { get; set; }
    }
}
