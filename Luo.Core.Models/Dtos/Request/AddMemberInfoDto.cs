using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luo.Core.Models.Dtos.Request
{
    public class AddMemberInfoDto
    {
        public string MemberName { get; set; }
        public string Password { get; set; }
        public string CreateName { get; set; }
    }
}
