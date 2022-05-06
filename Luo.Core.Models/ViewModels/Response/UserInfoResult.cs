using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luo.Core.Models.ViewModels.Response
{
    public class UserInfoResult
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public DateTime CreateTime { get; set; }
        public string CreateName { get; set; }
    }

   

}
