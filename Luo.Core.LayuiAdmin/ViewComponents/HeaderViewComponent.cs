using Microsoft.AspNetCore.Mvc;

namespace Luo.Core.LayuiAdmin.ViewComponents
{
    public class HeaderViewComponent : ViewComponent
    {
        public HeaderViewComponent()
        {

        }
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
