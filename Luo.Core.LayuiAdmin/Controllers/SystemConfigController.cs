using Luo.Core.FiltersExtend;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Luo.Core.LayuiAdmin.Controllers
{
    [Authorize(Policy = GlobalVars.PermissionsName)]
    public class SystemConfigController : Controller
    {
       
        public IActionResult UserManage()
        {
            return View();
        }
    }
}
