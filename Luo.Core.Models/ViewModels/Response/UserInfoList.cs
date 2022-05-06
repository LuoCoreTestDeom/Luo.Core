using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luo.Core.Models.ViewModels.Response
{
  
    public class UserInfoList
    {
        public List<UserInfoResult> UserInfos { get; set; }
        public int TotalCount { get; set; }
    }
}
