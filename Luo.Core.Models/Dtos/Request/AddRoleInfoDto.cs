using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luo.Core.Models.Dtos.Request
{
    public class AddRoleInfoDto
    {
        public string RoleName { get; set; }
        public List<int> MenuIds { get; set; }
    }
}
