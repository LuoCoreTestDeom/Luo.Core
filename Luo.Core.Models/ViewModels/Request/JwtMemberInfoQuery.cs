﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luo.Core.Models.ViewModels.Request
{
    public class JwtMemberInfoQuery
    {
        public string MemberName { get; set; }
        public string MemberPassword { get; set; }
    }
}