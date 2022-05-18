using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luo.Core.Models.Dtos.Response
{
    public class MemberInfoListDto
    {
        public int MemberId { get; set; }
        public string MemberName { get; set; }
        public string MemberPassword { get; set; }
        public DateTime CreateTime { get; set; }
        public string CreateName { get; set; }
   
    }
}
