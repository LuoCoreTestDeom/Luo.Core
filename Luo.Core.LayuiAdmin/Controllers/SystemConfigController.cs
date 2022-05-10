﻿using Luo.Core.FiltersExtend;
using Luo.Core.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Luo.Core.Common;
using Luo.Core.Models.ViewModels.Request;
using MySqlX.XDevAPI.Common;
using Luo.Core.Models.Dtos.Response;
using Org.BouncyCastle.Ocsp;

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
        [HttpDelete]
        public IActionResult DeleteUser(List<int> req)
        {
            var res = _systemConfigService.DeleteUserByUserIds(req);
            return Json(res);
        }

        public IActionResult MenuManage()
        {

            return View();
        }
        [HttpPost]
        public IActionResult GetMenuInfos()
        {
            var res = _systemConfigService.GetMenuInfoList();
            return Json(res);
        }

        public IActionResult MenuInfo()
        {
            return View();
        }
        [HttpPost]
        public IActionResult GetAllMenuInfos()
        {
            var res = _systemConfigService.GetMenuInfos();

            return Json(res);
        }

        [HttpPost]
        public IActionResult AddEditMenuInfo(MenuInfoInput req)
        {
            req.MenuEnable = !req.MenuEnable;
            var res = _systemConfigService.AddEditMenuInfo(req);
            return Json(res);
        }
        [HttpDelete]
        public IActionResult DeleteMenuInfoByIds(List<int> ids)
        {
            var res = _systemConfigService.DeleteMenuInfoByIds(ids);
            return Json(res);
        }

        public IActionResult RoleManage()
        {
            return View();
        }
        [HttpPost]
        public IActionResult GetPageRoleInfo(RoleInfoPageQuery req)
        {
            var res = _systemConfigService.QueryRolePage(req);
            return Json(res);
        }

        public IActionResult RoleInfo()
        {
            return View();
        }
    }
}
