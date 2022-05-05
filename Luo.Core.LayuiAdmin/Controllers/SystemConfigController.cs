using Luo.Core.FiltersExtend;
using Luo.Core.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Luo.Core.Common;

namespace Luo.Core.LayuiAdmin.Controllers
{
    [Authorize(Policy = GlobalVars.PermissionsName)]
    public class SystemConfigController : Controller
    {
        private readonly ISystemConfigService _systemConfigService;

        public SystemConfigController(Luo.Core.IServices.ISystemConfigService systemConfigService)
        {
            this._systemConfigService = systemConfigService;
        }
       
        public IActionResult UserManage()
        {
            return View();
        }
        [HttpPost]
        public IActionResult QueryUserList(Luo.Core.Models.ViewModels.Request.QueryUserInfoViewModel req) 
        {
            var res = _systemConfigService.QueryUserInfoList(req);
            
            return  Json(res);
        }

        public IActionResult AddUser() 
        {
            return View();
        }
    }
}
