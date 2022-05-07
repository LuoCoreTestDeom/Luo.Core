using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luo.Core.Models.ViewModels.Response
{
    public class UserMenuInfoOutput: MenuInfoList
    {
        public List<UserMenuInfoOutput> ChildrenMeuns { get; set; }
    }
}
