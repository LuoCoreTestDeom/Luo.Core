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
                if (db.Queryable<Basic_User>().Any(x => x.UserName == req.UserName))
                {
                    res.Message = "用户名已存在";
                    return;
                }
                res.Status = db.Insertable<Basic_User>(new
                {
                    UserName = req.UserName,
                    Password = req.Password,
                    CreateName = req.CreateName,
                    CreateTime = DateTime.Now
                }).ExecuteCommand() > 0;

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
                res.Status = db.Updateable<Basic_User>(new
                {
                    UserName = req.UserName,
                }).Where(x=>x.Id==req.UserId).ExecuteCommand() > 0;

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
    }
}