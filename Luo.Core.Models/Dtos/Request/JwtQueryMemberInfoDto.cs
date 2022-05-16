using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luo.Core.Models.Dtos.Request
{
    public  class JwtQueryMemberInfoDto
    {
        public string MemberName { get; set; }
        public string MemberPassword { get; set; }
    }
}
