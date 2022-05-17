using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luo.Core.Models.ViewModels.Request
{
    public class AddEditMemberInfoInput
    {
        public int MemberId { get; set; }
        public string MemberName { get; set; }
        public string Password { get; set; }
        public string MemberConfirmPassword { get; set; }
        public int MemberSex { get; set; }
    }
}
