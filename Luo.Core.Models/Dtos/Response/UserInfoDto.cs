﻿using Luo.Core.Models.ViewModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luo.Core.Models.Dtos.Response
{
    public class UserInfoDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public DateTime CreateTime { get; set; }
        public string CreateName { get; set; }
    }
    public class UserInfoListDto
    {
        public List<UserInfoDto> UserInfoList { get; set; }
        public int TotalCount { get; set; }
    }
}
