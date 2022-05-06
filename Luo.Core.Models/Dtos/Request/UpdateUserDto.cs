using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luo.Core.Models.Dtos.Request
{
    public class UpdateUserDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int Sex { get; set; }
    }
}
