using AutoMapper;
using Luo.Core.DatabaseEntity;
using Luo.Core.DatabaseFactory;
using Luo.Core.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luo.Core.Services
{
    public class DatabaseInitDataService : SqlSugarRepositoryList<ISqlSugarFactory, IBasicRepository>, IServices.IDatabaseInitDataService
    {
        public DatabaseInitDataService(ISqlSugarFactory factory, IBasicRepository rep, IMapper mapper) : base(factory, rep, mapper)
        {
        }




        /// <summary>
        /// 添加初始化数据
        /// </summary>
        /// <returns></returns>
        public bool AddInitUser()
        {
            bool res = false;
            string strValue = Luo.Core.Common.SecurityEncryptDecrypt.CommonUtil.EncryptString("123456");
            Factory.GetDbContext((db) =>
            {
                db.Deleteable<Basic_User>().Where(x => x.UserName == "Luo").ExecuteCommand();
                res = db.Insertable<Basic_User>(new
                {
                    UserName = "Luo",
                    Password = strValue,
                    CreateTime = DateTime.Now,
                    CreateName = "LuoCore"
                }).ExecuteCommand() > 0;
                //for (int i = 0; i < 9999; i++)
                //{
                //    db.Insertable<Basic_User>(new
                //    {
                //        UserName = "Luo" + i,
                //        Password = strValue,
                //        CreateTime = DateTime.Now,
                //        CreateName = "LuoCore" + i
                //    }).ExecuteCommand();
                //}
            });
            return res;
        }
        /// <summary>
        /// 添加初始化数据
        /// </summary>
        /// <returns></returns>
        public bool AddInitMenu()
        {
            bool res = false;
            Factory.GetDbContext((db) =>
            {
                try
                {
                    db.BeginTran();
                    db.Deleteable<Basic_Menu>().Where(x => x.MenuName == "系统管理").ExecuteCommand();
                    db.Deleteable<Basic_Menu>().Where(x => x.MenuName == "用户管理").ExecuteCommand();
                    db.Deleteable<Basic_Menu>().Where(x => x.MenuName == "菜单管理").ExecuteCommand();
                    var menuId = db.Insertable<Basic_Menu>(new
                    {
                        MenuName = "系统管理",
                        MenuEnable = true,
                        MenuType = 0,
                        ParentMenuId = 0,
                        MenuSort = 1,
                        MenuIcon = "layui-icon-component",
                        MenuAddress = ""
                    }).ExecuteReturnIdentity();
                    db.Insertable<Basic_Menu>(new
                    {
                        MenuName = "用户管理",
                        MenuEnable = true,
                        MenuType = 1,
                        ParentMenuId = menuId,
                        MenuSort = 1,
                        MenuIcon = "layui-icon-component",
                        MenuAddress = "/SystemConfig/UserManage"
                    }).ExecuteCommand();
                    db.Insertable<Basic_Menu>(new
                    {
                        MenuName = "菜单管理",
                        MenuEnable = true,
                        MenuType = 1,
                        ParentMenuId = menuId,
                        MenuSort = 2,
                        MenuIcon = "layui-icon-component",
                        MenuAddress = "/SystemConfig/MenuPermissionManagement"
                    }).ExecuteCommand();
                    db.CommitTran();
                    res = true;
                }
                catch (Exception)
                {
                    db.RollbackTran();
                }


            });
            return res;
        }


        public bool AddInitRole()
        {
            bool res = false;
            Factory.GetDbContext((db) =>
            {
                try
                {
                    db.BeginTran();
                    db.Deleteable<Basic_Role>().Where(x => x.RoleName == "管理员").ExecuteCommand();
                    db.Insertable<Basic_Role>(new
                    {
                        RoleName = "管理员"
                    }).ExecuteCommand();
                    db.CommitTran();
                    res = true;
                }
                catch (Exception)
                {
                    db.RollbackTran();
                }

            });
            return res;
        }

        public bool AddInitTableConcern()
        {
            bool res = false;
            Factory.GetDbContext((db) =>
            {
                try
                {
                    db.BeginTran();

                    var userInfo = db.Queryable<Basic_User>().Where(x => x.UserName == "Luo").First();
                    var roleInfo = db.Queryable<Basic_Role>().Where(x => x.RoleName == "管理员").First();
                    var menuInfos = db.Queryable<Basic_Menu>().ToList();
                    db.Deleteable<Basic_MenuRole>().ExecuteCommand();
                    db.Deleteable<Basic_UserRole>().ExecuteCommand();
                    foreach (var item in menuInfos)
                    {
                        db.Insertable<Basic_MenuRole>(new
                        {
                            RoleId = roleInfo.Id,
                            MenuId = item.Id
                        }).ExecuteCommand();
                    }
                    db.Insertable<Basic_UserRole>(new
                    {
                        RoleId = roleInfo.Id,
                        UserId = userInfo.Id
                    }).ExecuteCommand();
                    db.CommitTran();
                    res = true;
                }
                catch (Exception)
                {
                    db.RollbackTran();
                }

            });
            return res;
        }
    }
}
