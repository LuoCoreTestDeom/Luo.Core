using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Luo.Core.LayuiAdmin.Controllers
{
   
    public class UserController : Controller
    {
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
    }
}
