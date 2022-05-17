using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luo.Core.Models.Dtos.Request
{
    public class UpdateMemberInfoDto
    {
        public int MemberId { get; set; }
        public string MemberName { get; set; }
    }
}
