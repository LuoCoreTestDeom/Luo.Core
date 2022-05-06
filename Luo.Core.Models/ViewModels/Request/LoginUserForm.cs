using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luo.Core.Models.ViewModels.Request
{
    public class LoginUserForm
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Vercode { get; set; }
    }
}
