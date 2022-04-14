using Microsoft.AspNetCore.Mvc;

namespace Luo.Core.LayuiAdmin.ViewComponents
{
    public class SideMenuViewComponent : ViewComponent
    {
        public SideMenuViewComponent()
        {

        }
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
