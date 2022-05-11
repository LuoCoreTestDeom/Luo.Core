using Luo.Core.FiltersExtend;
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
        /// <summary>
        /// 用户管理
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// 用户信息弹框
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// 菜单管理
        /// </summary>
        /// <returns></returns>
        public IActionResult MenuManage()
        {

            return View();
        }
        /// <summary>
        /// 查询所有菜单信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetMenuInfos()
        {
            var res = _systemConfigService.GetMenuInfoList();
            return Json(res);
        }
        /// <summary>
        /// 菜单信息弹框
        /// </summary>
        /// <returns></returns>
        public IActionResult MenuInfo()
        {
            return View();
        }
        /// <summary>
        /// 获取所有菜单--递归
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetAllMenuInfos()
        {
            var res = _systemConfigService.GetMenuInfos();

            return Json(res);
        }
       /// <summary>
       /// 添加编辑菜单
       /// </summary>
       /// <param name="req"></param>
       /// <returns></returns>
        [HttpPost]
        public IActionResult AddEditMenuInfo(MenuInfoInput req)
        {
            req.MenuEnable = !req.MenuEnable;
            var res = _systemConfigService.AddEditMenuInfo(req);
            return Json(res);
        }
        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        public IActionResult DeleteMenuInfoByIds(List<int> ids)
        {
            var res = _systemConfigService.DeleteMenuInfoByIds(ids);
            return Json(res);
        }
       /// <summary>
       /// 角色管理
       /// </summary>
       /// <returns></returns>
        public IActionResult RoleManage()
        {
            return View();
        }
       /// <summary>
       /// 查询角色信息
       /// </summary>
       /// <param name="req"></param>
       /// <returns></returns>
        [HttpPost]
        public IActionResult GetPageRoleInfo(RoleInfoPageQuery req)
        {
            var res = _systemConfigService.QueryRolePage(req);
            return Json(res);
        }
        /// <summary>
        /// 角色信息弹框
        /// </summary>
        /// <returns></returns>
        public IActionResult RoleInfo()
        {
            return View();
        }
        /// <summary>
        /// 获取角色菜单明细
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetRoleMenuInfo(int roleId)
        {
            var res = _systemConfigService.QueryRoleMenuInfo(roleId);
            return Json(res);
        }


        /// <summary>
        /// 修改增加角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddEditRoleInfo(AddEditRoleInfoInput req)
        {
            var res = _systemConfigService.AddEditRoleInfo(req);
            return Json(res);
        }


        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpDelete]
        public IActionResult DeleteRoleInfoByIds(List<int> req)
        {
            var res = _systemConfigService.DeleteRoleInfoByIds(req);
            return Json(res);
        }
    }
}
