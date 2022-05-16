using Luo.Core.IServices;
using Luo.Core.Models.ViewModels.Request;
using Microsoft.AspNetCore.Mvc;

namespace Luo.Core.LayuiAdmin.Controllers
{
    public class MemberController : Controller
    {
        readonly IMemberService _service;
        public MemberController(IMemberService service)
        {
             _service = service;
        }
        public IActionResult MemberManage()
        {
            return View();
        }
        [HttpPost]
        public IActionResult QueryMemberInfo(MemberInfoPageQuery req) 
        {
           var res= _service.QueryMemberInfoPageList(req);
            return Json(res);
        }
    }
}
