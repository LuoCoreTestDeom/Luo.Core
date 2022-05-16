using Microsoft.AspNetCore.Mvc;

namespace Luo.Core.LayuiAdmin.Controllers
{
    public class MemberController : Controller
    {
        public IActionResult MemberManage()
        {
            return View();
        }
        public IActionResult QueryMemberInfo() 
        {
            return View();
        }
    }
}
