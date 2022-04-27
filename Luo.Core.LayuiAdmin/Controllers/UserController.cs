
using AutoMapper;
using Luo.Core.Common;
using Luo.Core.Common.CaptchaVerificationCode;
using Luo.Core.Models.Dtos.Request;
using Luo.Core.Models.ViewModels;
using Luo.Core.Models.ViewModels.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetTaste;
using System.Net.Http.Headers;

namespace Luo.Core.LayuiAdmin.Controllers
{

    public class UserController : Controller
    {
        private readonly IServices.IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IServices.IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpGet]
        public async Task<FileContentResult> CaptchaAsync([FromServices] ICaptcha _captcha)
        {
            var code = await _captcha.GenerateRandomCaptchaAsync();
            this.HttpContext.SetSessionString("UserLoginCaptcha", code);
            var result = await _captcha.GenerateCaptchaImageAsync(code);
            return File(result.CaptchaMemoryStream.ToArray(), "image/png");
        }
        [HttpPost]
        public IActionResult Login(UserLoginViewModel req, string callback)
        {
            CommonViewModel res = new CommonViewModel();
            if (string.IsNullOrWhiteSpace(req.UserName))
            {
                res.Msg = "用户名称不能为空！";
            }
            else if (string.IsNullOrWhiteSpace(req.Password))
            {
                res.Msg = "密码不能为空！";
                return Json(res);
            }
            else
            {
                var captchaCode = this.HttpContext.GetSessionString("UserLoginCaptcha");
                if (captchaCode.Equals(req.Vercode))
                {
                    var result = _userService.UserLogin(req);
                   res= _mapper.Map<CommonViewModel>(result);
                }
                else 
                {
                    res.Msg = "验证码错误！";
                }
            }
           

            return Content(String.Format("{0}({1});", callback, res.ObjToJson()), "application/javascript");//返回jsonp
        }
    }
}
