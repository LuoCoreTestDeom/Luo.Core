using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luo.Core.Models.ViewModels.Response
{
    public class MemberInfoPageList
    {
        public List<MemberInfoResult> MemberInfos { get; set; }
        public int TotalCount { get; set; }
    }
}
