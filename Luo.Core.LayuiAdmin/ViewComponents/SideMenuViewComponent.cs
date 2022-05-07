using Luo.Core.Common;
using Luo.Core.Models.Dtos.Response;
using Microsoft.AspNetCore.Mvc;

namespace Luo.Core.LayuiAdmin.ViewComponents
{
    public class SideMenuViewComponent : ViewComponent
    {
        IServices.ISharedService _service;
        public SideMenuViewComponent(IServices.ISharedService service)
        {
            _service = service;
        }
        public IViewComponentResult Invoke()
        {
            var userInfo = this.HttpContext.GetCookie<LoginUserInfoDto>("UserInfo");
           var res= _service.GetUserMenuInfos(userInfo.UserId);
            return View(res);
        }
    }
}
