using Microsoft.AspNetCore.Mvc;

namespace Luo.Core.LayuiAdmin.ViewComponents
{
    public class TabsViewComponent : ViewComponent
    {
        public TabsViewComponent()
        {

        }
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
