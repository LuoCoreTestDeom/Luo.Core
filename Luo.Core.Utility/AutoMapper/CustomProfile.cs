using AutoMapper;
using Luo.Core.Models.Dtos;
using Luo.Core.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luo.Core.Common;
using Luo.Core.Models.Dtos.Request;
using Luo.Core.Models.Dtos.Response;
using Org.BouncyCastle.Ocsp;
using Luo.Core.Models.ViewModels.Request;
using System.ComponentModel;
using System.Reflection;
using System.Xml.Linq;
using static Luo.Core.Utility.AutoMapper.CustomProfile;
using Luo.Core.Models.ViewModels.Response;
using Luo.Core.DatabaseEntity;
using MySqlX.XDevAPI.Common;

namespace Luo.Core.Utility.AutoMapper
{
    public class CustomProfile : Profile
    {
        /// <summary>
        /// 配置构造函数，用来创建关系映射
        /// </summary>
        public CustomProfile()
        {
            CreateMap<CommonDto, CommonViewModel>();


            

            CreateMap<LoginUserForm, LoginUserDto>();
            CreateMap<UserInfoQuery, QueryUserInfoDto>();




            CreateMap(typeof(CommonViewModel<>), typeof(CommonViewModel))
                .ConvertUsing(typeof(TypeTypeConverter<>));

            CreateMap(typeof(CommonDto<>), typeof(CommonViewModel<>))
                .ConvertUsing(typeof(CommonTypeConverter<>));

            CreateMap<UserInfoDto, UserInfoResult>()
                .ForMember(dest => dest.UserId, opts => opts.MapFrom(x => x.UserId))
                .ForMember(dest => dest.UserName, opts => opts.MapFrom(x => x.UserName))
                .ForMember(dest => dest.CreateName, opts => opts.MapFrom(x => x.CreateName))
                .ForMember(dest => dest.CreateTime, opts => opts.MapFrom(x => x.CreateTime));
            CreateMap<UserInfoListDto, UserInfoList>()
                .ForMember(dest => dest.UserInfos, opts => opts.MapFrom(x => x.UserInfoList))
                .ForMember(dest => dest.TotalCount, opts => opts.MapFrom(x => x.TotalCount));

         
            CreateMap<UserInfoInput,AddUserDto>()
                .ForMember(dest => dest.Sex, opts => opts.MapFrom(x => x.UserSex));

            CreateMap<UserInfoInput, UpdateUserDto>()
               .ForMember(dest => dest.Sex, opts => opts.MapFrom(x => x.UserSex));

            CreateMap<MenuInfoDto,MenuInfoList>()
                .ForMember(dest => dest.MenuUrl, opts => opts.MapFrom(x => x.MenuAddress));

           

            CreateMap<MenuInfoDto, UserMenuInfoOutput>()
               .ForMember(dest => dest.MenuUrl, opts => opts.MapFrom(x => x.MenuAddress));

            CreateMap<MenuInfoDto, MenuGroupInfoResult>()
              .ForMember(dest => dest.MenuUrl, opts => opts.MapFrom(x => x.MenuAddress));

            CreateMap<MenuInfoList, UserMenuInfoOutput>();

            CreateMap<MenuInfoInput,AddMenuInfoDto>()
            .ForMember(dest => dest.MenuType, opts => opts.MapFrom(x => x.MenuType.ObjToInt()));

            //CreateMap<SysUserInfoDto, SysUserInfo>()
            //    .ForMember(a => a.Id, o => o.MapFrom(d => d.uID))
            //    .ForMember(a => a.Address, o => o.MapFrom(d => d.addr))
            //    .ForMember(a => a.RIDs, o => o.MapFrom(d => d.RIDs))
            //    .ForMember(a => a.Age, o => o.MapFrom(d => d.age))
            //    .ForMember(a => a.Birth, o => o.MapFrom(d => d.birth))
            //    .ForMember(a => a.Status, o => o.MapFrom(d => d.uStatus))
            //    .ForMember(a => a.UpdateTime, o => o.MapFrom(d => d.uUpdateTime))
            //    .ForMember(a => a.CreateTime, o => o.MapFrom(d => d.uCreateTime))
            //    .ForMember(a => a.ErrorCount, o => o.MapFrom(d => d.uErrorCount))
            //    .ForMember(a => a.LastErrorTime, o => o.MapFrom(d => d.uLastErrTime))
            //    .ForMember(a => a.LoginName, o => o.MapFrom(d => d.uLoginName))
            //    .ForMember(a => a.LoginPWD, o => o.MapFrom(d => d.uLoginPWD))
            //    .ForMember(a => a.Remark, o => o.MapFrom(d => d.uRemark))
            //    .ForMember(a => a.RealName, o => o.MapFrom(d => d.uRealName))
            //    .ForMember(a => a.Name, o => o.MapFrom(d => d.name))
            //    .ForMember(a => a.IsDeleted, o => o.MapFrom(d => d.tdIsDelete))
            //    .ForMember(a => a.RoleNames, o => o.MapFrom(d => d.RoleNames));
        }

        public class TypeTypeConverter<T> : ITypeConverter<CommonViewModel<T>, CommonViewModel>
        {
            public CommonViewModel Convert(CommonViewModel<T> source, CommonViewModel destination, ResolutionContext context)
            {
                return new CommonViewModel()
                {
                    Status = source.Status,
                    StatusCode = source.StatusCode,
                    Msg = source.Msg,
                    ResultData = source.ResultData.ObjToJson()
                };
            }
        }

        public class CommonTypeConverter<T> : ITypeConverter<CommonDto<T>, CommonViewModel<T>>
        {
            public CommonViewModel<T> Convert(CommonDto<T> source, CommonViewModel<T> destination, ResolutionContext context)
            {
                 destination= new CommonViewModel<T>()
                {
                    Status = source.Status,
                    StatusCode = source.StatusCode,
                    Msg = source.Message,
                    ResultData = source.ResultData
                };
                return destination;
            }

          
        }


    }
}
