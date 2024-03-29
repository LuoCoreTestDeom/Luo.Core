﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luo.Core.Models.Dtos.Request
{
    public class QueryUserInfoDto
    {
        public string UserName { get; set; }
        public bool TimeEnable { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
        public int PageIndex { get; set; }
        public int PageCount { get; set; }
    }
}
