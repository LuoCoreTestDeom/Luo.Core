using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luo.Core.Models.ViewModels.Request
{
    public class MemberInfoPageQuery
    {
        public string MemberName { get; set; }
        public bool TimeEnable { get; set; }

        public DateTime TimeStrart { get; set; }
        public DateTime TimeEnd { get; set; }
        public int PageIndex { get; set; }
        public int PageCount { get; set; }
    }
}
