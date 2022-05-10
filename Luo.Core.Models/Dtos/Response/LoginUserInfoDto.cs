using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luo.Core.Models.Dtos.Response
{
    public class LoginUserInfoDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public List<RoleInfoDto> RoleInfos { get; set; }

    }
 
}
