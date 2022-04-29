using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luo.Core.Models.ViewModels.Response
{
    public class UserInfoViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public DateTime CreateTime { get; set; }
        public string CreateName { get; set; }
    }

    public class UserInfoListViewModel
    {
        public List<UserInfoViewModel> UserInfoList { get; set; }
        public int TotalCount { get; set; }
    }

}
