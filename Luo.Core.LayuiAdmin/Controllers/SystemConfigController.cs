using Luo.Core.FiltersExtend;
using Luo.Core.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Luo.Core.Common;
using Luo.Core.Models.ViewModels.Request;
using MySqlX.XDevAPI.Common;
using Luo.Core.Models.Dtos.Response;

namespace Luo.Core.LayuiAdmin.Controllers
{
    [Authorize(Policy = GlobalVars.PermissionsName)]
    public class SystemConfigController : Controller
    {
        private readonly ISystemConfigService _systemConfigService;

        public SystemConfigController(Luo.Core.IServices.ISystemConfigService systemConfigService)
        {
            this._systemConfigService = systemConfigService;
        }

        public IActionResult UserManage()
        {
            return View();
        }
        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult QueryUserList(UserInfoQuery req)
        {
            var res = _systemConfigService.QueryUserInfoList(req);

            return Json(res);
        }

        public IActionResult UserInfo()
        {
            return View();
        }
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddUser(UserInfoInput req)
        {
            var res = _systemConfigService.AddUser(req);
            return Json(res);
        }
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UpdateUser(UserInfoInput req)
        {
            var res = _systemConfigService.UpdateUser(req);
            return Json(res);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult DeleteUser(List<int> req)
        {
            var res = _systemConfigService.DeleteUserByUserIds(req);
            return Json(res);
        }
    }
}
