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
            return View();
        }
    }
}
