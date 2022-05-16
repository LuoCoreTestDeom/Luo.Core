using Luo.Core.DatabaseEntity;
using Luo.Core.DatabaseFactory;
using Luo.Core.IRepository;
using Luo.Core.Models.Dtos;
using Luo.Core.Models.Dtos.Request;
using Luo.Core.Models.Dtos.Response;
using Org.BouncyCastle.Ocsp;
using SqlSugar;

namespace Luo.Core.Repository
{
    public class BasicRepository : SqlSugarRepositoryList<ISqlSugarFactory>, IBasicRepository
    {
        public BasicRepository(ISqlSugarFactory factory, AutoMapper.IMapper mapper) : base(factory, mapper)
        {
        }



        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <returns></returns>
        public LoginUserInfoDto QueryUserInfo(LoginUserDto req)
        {
            LoginUserInfoDto res = new LoginUserInfoDto();

            Factory.GetDbContext((db) =>
            {
                var result = db.Queryable<Basic_User>()
                .LeftJoin<Basic_UserRole>((a, b) => b.UserId == a.Id)
                .LeftJoin<Basic_Role>((a, b, c) => c.Id == b.RoleId)
                .Where((a, b, c) => a.UserName == req.UserName && a.Password == req.Password)
                .Select((a, b, c) => new
                {
                    UserId = a.Id,
                    UserName = a.UserName,
                    RoleId = c.Id,
                    RoleName = c.RoleName
                }).ToList();
                if (result != null && result.Count > 0)
                {
                    res.UserId = result[0].UserId;
                    res.UserName = result[0].UserName;
                    res.RoleInfos = new List<RoleInfoDto>();
                    foreach (var item in result)
                    {
                        if (item.RoleId > 0)
                        {
                            res.RoleInfos.Add(new RoleInfoDto()
                            {
                                RoleId = item.RoleId,
                                RoleName = item.RoleName
                            });
                        }

                    }
                }

            });
            return res;
        }
        /// <summary>
        /// 查询所有用户信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public UserInfoListDto QueryUserInfoList(QueryUserInfoDto req)
        {
            UserInfoListDto res = new UserInfoListDto();
            Factory.GetDbContext((db) =>
            {
                int totleCount = 0;
                res.UserInfoList = db.Queryable<Basic_User>()
                .WhereIF(!string.IsNullOrWhiteSpace(req.UserName), x => x.UserName.Contains(req.UserName))
                .WhereIF(req.TimeEnable, x => SqlFunc.Between(x.CreateTime, req.TimeStart, req.TimeEnd))
                 .Select(x => new UserInfoDto
                 {
                     UserId = x.Id,
                     UserName = x.UserName,
                     CreateTime = x.CreateTime,
                     CreateName = x.CreateName
                 })
                .ToPageList(req.PageIndex, req.PageCount, ref totleCount);

                res.TotalCount = totleCount;
            });
            return res;
        }
        /// <summary>
        /// 添加用户信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public CommonDto AddUser(AddUserDto req)
        {
            CommonDto res = new CommonDto();
            Factory.GetDbContext((db) =>
            {

                try
                {
                    db.BeginTran();
                    if (db.Queryable<Basic_User>().Any(x => x.UserName == req.UserName))
                    {
                        res.Message = "用户名已存在";
                        return;
                    }
                    var userId = db.Insertable<Basic_User>(new
                    {
                        UserName = req.UserName,
                        Password = req.Password,
                        CreateName = req.CreateName,
                        CreateTime = DateTime.Now
                    }).ExecuteReturnIdentity();
                    foreach (var item in req.RoleIds)
                    {
                        db.Insertable<Basic_UserRole>(new 
                        {
                            UserId = userId,
                            RoleId = item
                        }).ExecuteCommand();
                    }
                    db.CommitTran();
                    res.Status = true;
                }
                catch (Exception ex)
                {
                    db.RollbackTran();
                    res.Message=ex.Message;
                }


            });
            return res;
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public CommonDto UpdateUser(UpdateUserDto req)
        {
            CommonDto res = new CommonDto();
            Factory.GetDbContext((db) =>
            {

                try
                {
                    db.BeginTran();
                    db.Updateable<Basic_User>(new
                    {
                        UserName = req.UserName,
                    }).Where(x => x.Id == req.UserId).ExecuteCommand();
                    db.Deleteable<Basic_UserRole>().Where(x => x.UserId == req.UserId).ExecuteCommand();
                    foreach (var item in req.RoleIds)
                    {
                        db.Insertable<Basic_UserRole>(new
                        {
                            UserId = req.UserId,
                            RoleId = item
                        }).ExecuteCommand();
                    }
                    db.CommitTran();
                    res.Status = true;
                }
                catch (Exception ex)
                {
                    db.RollbackTran();
                    res.Message = ex.Message;
                }


            });
            return res;
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        public CommonDto DeleteUserByUserIds(List<int> userIds)
        {
            CommonDto res = new CommonDto();
            Factory.GetDbContext((db) =>
            {
                try
                {
                    db.BeginTran();
                    foreach (var item in userIds)
                    {
                        db.Deleteable<Basic_User>().Where(x => x.Id == item).ExecuteCommand();
                    }
                    db.CommitTran();
                    res.Status = true;
                }
                catch (Exception ex)
                {
                    db.RollbackTran();
                    res.Message = "删除发生异常：" + ex.Message;
                }
            });
            return res;
        }

        /// <summary>
        /// 获取用户绑定的所有角色
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<int> GetUserRoleIdsByUserId(int userId)
        {
            List<int> res = new List<int>();
            Factory.GetDbContext((db) =>
            {
                res = db.Queryable<Basic_UserRole>()
                .Where(a => a.UserId == userId)
                 .Select(a => a.RoleId).ToList();
            });
            return res;
        }




        /// <summary>
        /// 查询菜单列表信息
        /// </summary>
        /// <returns></returns>
        public List<MenuInfoDto> QueryMenuList()
        {
            List<MenuInfoDto> res = new List<MenuInfoDto>();
            Factory.GetDbContext((db) =>
            {
                res = db.Queryable<Basic_Menu>().Select(x => new MenuInfoDto()
                {
                    MenuId = x.Id,
                    MenuName = x.MenuName,
                    MenuAddress = x.MenuAddress,
                    MenuType = x.MenuType,
                    MenuIcon = x.MenuIcon,
                    MenuSort = x.MenuSort,
                    MenuEnable = x.MenuEnable,
                    ParentMenuId = x.ParentMenuId

                }).ToList();
            });
            return res;
        }


        /// <summary>
        /// 通过用户ID获取菜单列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<MenuInfoDto> QueryMenuInfoListUserId(int userId)
        {
            List<MenuInfoDto> res = new List<MenuInfoDto>();
            Factory.GetDbContext((db) =>
            {

                res = db.Queryable<Basic_Menu>()
                 .LeftJoin<Basic_MenuRole>((a, b) => b.MenuId == a.Id)
                 .LeftJoin<Basic_UserRole>((a, b, c) => c.RoleId == b.RoleId)
                 .Where((a, b, c) => a.MenuEnable && c.UserId == userId)
                 .Distinct()
                 .Select((a, b) => new MenuInfoDto
                 {
                     MenuId = a.Id,
                     MenuName = a.MenuName,
                     MenuAddress = a.MenuAddress,
                     MenuIcon = a.MenuIcon,
                     MenuSort = a.MenuSort,
                     MenuType = a.MenuType,
                     ParentMenuId = a.ParentMenuId,
                     MenuEnable = a.MenuEnable
                 }).ToList();
            });
            return res;
        }

        /// <summary>
        /// 获取所有菜单列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<MenuInfoDto> QueryAllMenuInfoList()
        {
            List<MenuInfoDto> res = new List<MenuInfoDto>();
            Factory.GetDbContext((db) =>
            {

                res = db.Queryable<Basic_Menu>()
                 .Select(a => new MenuInfoDto
                 {
                     MenuId = a.Id,
                     MenuName = a.MenuName,
                     MenuAddress = a.MenuAddress,
                     MenuIcon = a.MenuIcon,
                     MenuSort = a.MenuSort,
                     MenuType = a.MenuType,
                     ParentMenuId = a.ParentMenuId,
                     MenuEnable = a.MenuEnable
                 }).ToList();
            });
            return res;
        }

        /// <summary>
        /// 添加菜单
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool AddMenuInfo(AddMenuInfoDto req)
        {
            bool res = false;
            Factory.GetDbContext((db) =>
            {
                res = db.Insertable<Basic_Menu>(new
                {
                    MenuName = req.MenuName,
                    MenuAddress = req.MenuAddress,
                    MenuIcon = req.MenuIcon,
                    MenuSort = req.MenuSort,
                    MenuType = req.MenuType,
                    ParentMenuId = req.ParentMenuId,
                    MenuEnable = req.MenuEnable
                }).ExecuteCommand() > 0;
            });
            return res;
        }

        /// <summary>
        /// 修改菜单
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool EditMenuInfo(EditMenuInfoDto req)
        {
            bool res = false;
            Factory.GetDbContext((db) =>
            {
                res = db.Updateable<Basic_Menu>(new
                {
                    MenuName = req.MenuName,
                    MenuAddress = req.MenuAddress,
                    MenuIcon = req.MenuIcon,
                    MenuSort = req.MenuSort,
                    MenuType = req.MenuType,
                    ParentMenuId = req.ParentMenuId,
                    MenuEnable = req.MenuEnable
                }).Where(x => x.Id == req.MenuId).ExecuteCommand() > 0;
            });
            return res;
        }

        /// <summary>
        /// 通过菜单ID 删除菜单信息
        /// </summary>
        /// <param name="menuIds"></param>
        /// <returns></returns>
        public CommonDto DeleteMenuInfoByIds(List<int> menuIds)
        {
            CommonDto res = new CommonDto();
            Factory.GetDbContext((db) =>
            {
                try
                {
                    db.BeginTran();
                    foreach (var item in menuIds)
                    {
                        db.Deleteable<Basic_Menu>().Where(x => x.Id == item).ExecuteCommand();
                    }
                    db.CommitTran();
                    res.Status = true;
                }
                catch (Exception ex)
                {
                    db.RollbackTran();
                    res.Message = ex.Message;
                }
            });
            return res;
        }


        /// <summary>
        /// 查询角色信息
        /// </summary>
        /// <param name="menuIds"></param>
        /// <returns></returns>
        public CommonPageDto<List<RoleInfoDto>> QueryRoleInfo(QueryRoleInfoDto req)
        {
            CommonPageDto<List<RoleInfoDto>> res = new CommonPageDto<List<RoleInfoDto>>();

            Factory.GetDbContext((db) =>
            {
                int totalCount = 0;
                res.ResultData = db.Queryable<Basic_Role>()
                .WhereIF(!string.IsNullOrWhiteSpace(req.RoleName), x => x.RoleName.Contains(req.RoleName))
                .Select(x => new RoleInfoDto
                {
                    RoleId = x.Id,
                    RoleName = x.RoleName
                }).ToPageList(req.PageIndex, req.PageCount, ref totalCount);
                res.TotalCount = totalCount;
            });
            return res;
        }

        /// <summary>
        /// 通过角色ID 获取菜单Ids
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public List<int> QueryRoleMenuIds(int roleId)
        {
            List<int> res = new List<int>();
            Factory.GetDbContext((db) =>
            {
                res = db.Queryable<Basic_MenuRole>()
                .Where(a => a.RoleId == roleId)
                 .Select(a => a.MenuId).ToList();
            });
            return res;
        }

        /// <summary>
        /// 查询所有角色信息
        /// </summary>
        /// <returns></returns>
        public List<QueryAllRoleInfoDto> QueryAllRoleInfos()
        {
            List<QueryAllRoleInfoDto> res = new List<QueryAllRoleInfoDto>();
            Factory.GetDbContext((db) =>
            {
                res = db.Queryable<Basic_Role>()
                .Select(x => new QueryAllRoleInfoDto
                {
                    RoleId = x.Id,
                    RoleName = x.RoleName
                }).ToList();
            });
            return res;
        }

        /// <summary>
        /// 新增一个角色
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public CommonDto AddRoleInfo(AddRoleInfoDto req)
        {
            CommonDto res = new CommonDto();
            Factory.GetDbContext((db) =>
            {

                try
                {
                    db.BeginTran();
                    var roleId = db.Insertable<Basic_Role>(new { RoleName = req.RoleName }).ExecuteReturnIdentity();

                    foreach (var item in req.MenuIds)
                    {
                        db.Insertable<Basic_MenuRole>(new
                        {
                            MenuId = item,
                            RoleId = roleId
                        }).ExecuteCommand();
                    }
                    db.CommitTran();
                    res.Status = true;
                }
                catch (Exception ex)
                {
                    res.Message = ex.Message;
                    db.RollbackTran();
                }
            });
            return res;
        }
        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public CommonDto EditRoleInfo(UpdateRoleInfoDto req)
        {
            CommonDto res = new CommonDto();
            Factory.GetDbContext((db) =>
            {

                try
                {
                    db.BeginTran();
                    db.Updateable<Basic_Role>(new { RoleName = req.RoleName }).Where(x => x.Id == req.RoleId).ExecuteCommand();
                    db.Deleteable<Basic_MenuRole>().Where(x => x.RoleId == req.RoleId).ExecuteCommand();
                    foreach (var item in req.MenuIds)
                    {
                        db.Insertable<Basic_MenuRole>(new
                        {
                            MenuId = item,
                            RoleId = req.RoleId
                        }).ExecuteCommand();
                    }
                    db.CommitTran();
                    res.Status = true;
                }
                catch (Exception ex)
                {
                    res.Message = ex.Message;
                    db.RollbackTran();
                }
            });
            return res;
        }


        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public CommonDto DeleteRoleInfoByIds(List<int> roleIds)
        {
            CommonDto res = new CommonDto();
            Factory.GetDbContext((db) =>
            {

                try
                {
                    db.BeginTran();
                    db.Deleteable<Basic_Role>().Where(x => roleIds.Contains(x.Id)).ExecuteCommand();
                    db.Deleteable<Basic_MenuRole>().Where(x => roleIds.Contains(x.RoleId)).ExecuteCommand();
                    db.CommitTran();
                    res.Status = true;
                }
                catch (Exception ex)
                {
                    res.Message = ex.Message;
                    db.RollbackTran();
                }
            });
            return res;
        }
        /// <summary>
        /// 获取JWT登录会员信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public JwtLoginMemberInfoDto QueryJwtMemberInfo(JwtQueryMemberInfoDto req) 
        {
            JwtLoginMemberInfoDto res = new JwtLoginMemberInfoDto();
            Factory.GetDbContext((db) =>
            {

                db.Queryable<Basic_Member>()
                .Where(x => x.MemberName == req.MemberName && x.Password == req.MemberPassword)
                .Select(x=>new JwtLoginMemberInfoDto 
                {
                    MemberName=x.MemberName
                })
                .First();
            });
            return res;
        }

      /// <summary>
      /// 查询会员信息
      /// </summary>
      /// <param name="req"></param>
      /// <returns></returns>
        public CommonPageDto<List<MemberInfoListDto>> QueryMemberInfoPageList(QueryMemberInfoPageDto req) 
        {
            CommonPageDto<List<MemberInfoListDto>> res = new CommonPageDto<List<MemberInfoListDto>>();
            Factory.GetDbContext((db) =>
            {
                int totalCount = 0;
                res.ResultData= db.Queryable<Basic_Member>()
                .WhereIF(!string.IsNullOrWhiteSpace(req.MemberName),x=>x.MemberName==req.MemberName)
                .WhereIF(req.TimeEnable, x => SqlFunc.Between(x.CreateTime,req.TimeStrart,req.TimeEnd))
                .Select(x => new MemberInfoListDto
                {
                    MemberId=x.Id,
                   MemberName=x.MemberName,
                   CreateTime=x.CreateTime,
                   CreateName=x.CreateName
                })
                .ToPageList(req.PageIndex, req.PageCount, ref totalCount);
                res.TotalCount = totalCount;
            });
            return res;
        }

    }
}