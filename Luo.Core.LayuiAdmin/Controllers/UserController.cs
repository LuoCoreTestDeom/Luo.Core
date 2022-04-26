
using Luo.Core.Common.CaptchaVerificationCode;
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
        [HttpGet]
        public async Task<FileContentResult> CaptchaAsync([FromServices] ICaptcha _captcha)
        {
            var code = await _captcha.GenerateRandomCaptchaAsync();

            var result = await _captcha.GenerateCaptchaImageAsync(code);

            return File(result.CaptchaMemoryStream.ToArray(), "image/png");
        }
    }
}
