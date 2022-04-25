using Luo.Core.Utility.Authorization.JsonWebToken;
using Luo.Core.Utility.Authorization.JsonWebToken.Secret;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Luo.Core.LayuiAdmin.Controllers
{
   
    public class UserController : Controller
    {
      
        public UserController()
        {
            
        }
        [AllowAnonymous]
        public IActionResult Login()
        {
           
            return View();
        }
    }
}
