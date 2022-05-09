
using AutoMapper;
using Luo.Core.Common;
using Luo.Core.Common.CaptchaVerificationCode;
using Luo.Core.FiltersExtend;
using Luo.Core.Models.Dtos.Request;
using Luo.Core.Models.ViewModels;
using Luo.Core.Models.ViewModels.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetTaste;
using System.Net.Http.Headers;
using System.Security.Claims;

using Luo.Core.Common.SecurityEncryptDecrypt;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

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
        public async Task<IActionResult> LoginAsync(LoginUserForm req, string callback)
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
                if (!string.IsNullOrWhiteSpace(captchaCode) && captchaCode.ToUpper().Equals(req.Vercode.ToUpper()))
                {
                    var result = _userService.UserLogin(req);
                    if (result.Status && result.ResultData != null && result.ResultData.UserId > 0)
                    {
                      
                        var claimIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                        claimIdentity.AddClaim(new Claim(ClaimTypes.Name, $"{result.ResultData.UserName}"));
                        claimIdentity.AddClaim(new Claim("Userid", result.ResultData.UserId.ObjToString()));
                        claimIdentity.AddClaim(new Claim("UserName", result.ResultData.UserName.ObjToString()));
                        claimIdentity.AddClaim(new Claim("SecurityKey", CommonUtil.EncryptString("LuoCore" + DateTime.Now.DateToTimeStamp())));
                        var menuInfo = _userService.GetUserMenuInfos(result.ResultData.UserId);
                        claimIdentity.AddClaim(new Claim("UserMenuList", menuInfo.ObjToJson()));
                        var userMenuInfos = _userService.RecursionMenu(menuInfo.Where(x => x.MenuType == 0).ToList(), menuInfo);
                        claimIdentity.AddClaim(new Claim("UserMenuInfo", userMenuInfos.ObjToJson()));

                        if (result.ResultData.RoleInfos != null)
                        {
                            foreach (var item in result.ResultData.RoleInfos)
                            {
                                claimIdentity.AddClaim(new Claim(ClaimTypes.Role, item.RoleName));
                                claimIdentity.AddClaim(new Claim("RoleId", item.RoleId.ObjToString()));
                            }
                        }


                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimIdentity), new AuthenticationProperties()
                        {
                            ExpiresUtc = DateTime.UtcNow.AddMinutes(Appsettings.GetValue("AuthCookieConfig", "ExpireTimeMinutes").ObjToMoney()),
                        });
                        res.Status = true;
                    }
                    else
                    {
                        res.Msg = "用户名或密码错误！";
                        res.Status = false;
                    }

                }
                else
                {
                    res.Msg = "验证码错误！";
                }
            }
           
            return Content(String.Format("{0}({1});", callback, res.ObjToJson()), "application/javascript");//返回jsonp
        }


        [HttpGet]
        public async Task<IActionResult> LoginOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            
            if (this.Request.IsAjax()) 
            {
                return Ok(Appsettings.GetValue("AuthCookieConfig", "LoginPath"));
            }
            else 
            {
                return RedirectPermanent(Appsettings.GetValue("AuthCookieConfig", "LoginPath"));
            }
        }
    }

    
}
