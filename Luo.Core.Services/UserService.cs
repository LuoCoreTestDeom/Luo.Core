using AutoMapper;
using Luo.Core.Common.SecurityEncryptDecrypt;
using Luo.Core.DatabaseFactory;
using Luo.Core.IRepository;
using Luo.Core.IServices;
using Luo.Core.Models.Dtos.Request;
using Luo.Core.Models.Dtos.Response;
using Luo.Core.Models.ViewModels;
using Luo.Core.Models.ViewModels.Request;
using Luo.Core.Models.ViewModels.Response;
using Luo.Core.Utility.AutoMapper;

namespace Luo.Core.Services
{
    public class UserService : SqlSugarRepositoryList<ISqlSugarFactory, IBasicRepository>, IServices.IUserService
    {
        public UserService(ISqlSugarFactory factory, IBasicRepository rep, IMapper mapper) : base(factory, rep, mapper)
        {
        }





        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public CommonViewModel<LoginUserInfoDto> UserLogin(LoginUserForm req)
        {
            CommonViewModel<LoginUserInfoDto> res = new CommonViewModel<LoginUserInfoDto>();
            var reqData = req.AutoMapTo<LoginUserDto>();
            try
            {
                reqData.Password = CommonUtil.EncryptString(reqData.Password);
                res.ResultData = _Rep.QueryUserInfo(reqData);
                res.Status = true;
            }
            catch (Exception ex)
            {
                res.Msg = ex.Message;
            }

            return res;
        }


        /// <summary>
        /// 获取用户的菜单列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<MenuInfoList> GetUserMenuInfos(int userId)
        {
           
           var resData= _Rep.QueryMenuInfoListUserId(userId);
            return _Mapper.Map<List<MenuInfoList>>(resData);
        }
    
        /// <summary>
        /// 递归遍历菜单
        /// </summary>
        /// <param name="resData"></param>
        /// <param name="sourceData"></param>
        /// <returns></returns>
        public List<UserMenuInfoOutput> RecursionMenu(List<MenuInfoList> resData, List<MenuInfoList> sourceData)
        {
            List<UserMenuInfoOutput> res = new List<UserMenuInfoOutput>();
            foreach (var item in resData)
            {
                var menuInfo = item.MapTo<UserMenuInfoOutput>();
                menuInfo.ChildrenMeuns = RecursionMenu(sourceData.Where(x => x.ParentMenuId == item.MenuId).ToList(), sourceData);
                res.Add(menuInfo);
            }
            return res;
        }


    }
}