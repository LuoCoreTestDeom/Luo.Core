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
using Luo.Core.Models.ViewModels.Response;
using Luo.Core.DatabaseEntity;
using MySqlX.XDevAPI.Common;
using Renci.SshNet.Common;
using Luo.Core.Utility.JsonWebToken.Dto;
using Luo.Core.Common.SecurityEncryptDecrypt;

namespace Luo.Core.Utility.AutoMapper
{
    public class CustomProfile : Profile
    {
        /// <summary>
        /// 配置构造函数，用来创建关系映射
        /// </summary>
        public CustomProfile()
        {

            CreateMap(typeof(CommonViewModel<>), typeof(CommonViewModel))
                .ConvertUsing(typeof(TypeTypeConverter<>));

            CreateMap(typeof(CommonDto<>), typeof(CommonViewModel<>))
                .ConvertUsing(typeof(CommonTypeConverter<>));
            CreateMap<CommonDto, CommonViewModel>()
                .ForMember(dest => dest.Msg, opts => opts.MapFrom(x => x.Message));
            CreateMap(typeof(CommonPageDto<>), typeof(CommonPageViewModel<>))
             .ConvertUsing(typeof(CommonPageTypeConverter<,>));

            CreateMap<UserInfoListDto, UserInfoList>()
                .ForMember(dest => dest.UserInfos, opts => opts.MapFrom(x => x.UserInfoList.MapToList<UserInfoResult>()))
                .ForMember(dest => dest.TotalCount, opts => opts.MapFrom(x => x.TotalCount));


            CreateMap<UserInfoInput, AddUserDto>()
                .ForMember(dest => dest.Sex, opts => opts.MapFrom(x => x.UserSex));

            CreateMap<UserInfoInput, UpdateUserDto>()
               .ForMember(dest => dest.Sex, opts => opts.MapFrom(x => x.UserSex));

            CreateMap<MenuInfoDto, MenuInfoList>()
                .ForMember(dest => dest.MenuUrl, opts => opts.MapFrom(x => x.MenuAddress));



            CreateMap<MenuInfoDto, UserMenuInfoOutput>()
               .ForMember(dest => dest.MenuUrl, opts => opts.MapFrom(x => x.MenuAddress));

            CreateMap<MenuInfoDto, MenuGroupInfoResult>()
              .ForMember(dest => dest.MenuUrl, opts => opts.MapFrom(x => x.MenuAddress));



            CreateMap<MenuInfoInput, AddMenuInfoDto>()
            .ForMember(dest => dest.MenuType, opts => opts.MapFrom(x => x.MenuType.ObjToInt()));
            CreateMap<MenuInfoInput, EditMenuInfoDto>()
           .ForMember(dest => dest.MenuType, opts => opts.MapFrom(x => x.MenuType.ObjToInt()));


            CreateMap<MenuInfoDto, TreeRoleMenuResult>()
          .ForMember(dest => dest.Id, opts => opts.MapFrom(x => x.MenuId))
           .ForMember(dest => dest.Title, opts => opts.MapFrom(x => x.MenuName));


          

            //CreateMap<SecretDto, JwtMemberInfoQuery>()
            //     .ForMember(dest => dest.MemberName, opts => opts.MapFrom(x => x.Account))
            //     .ForMember(dest => dest.MemberPassword, opts => opts.MapFrom(x => x.Password));



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
                destination = new CommonViewModel<T>()
                {
                    Status = source.Status,
                    StatusCode = source.StatusCode,
                    Msg = source.Message,
                    ResultData = source.ResultData
                };
                return destination;
            }


        }
        public class CommonPageTypeConverter<T, T2> : ITypeConverter<CommonPageDto<T>, CommonPageViewModel<T2>>
        {
            public CommonPageViewModel<T2> Convert(CommonPageDto<T> source, CommonPageViewModel<T2> destination, ResolutionContext context)
            {

                IMapper mapper = null;
                var typeSoure = typeof(T);
                var typeDest = typeof(T2);
                if (typeDest.IsGenericType)
                {
                    Type[] typeParametersSoure = typeSoure.GetGenericArguments();
                    Type[] typeParametersDest = typeDest.GetGenericArguments();
                    mapper = new MapperConfiguration(a => a.CreateMap(typeParametersSoure[0], typeParametersDest[0])).CreateMapper();

                }
                else
                {
                    mapper = new MapperConfiguration(a => a.CreateMap<T, T2>()).CreateMapper();
                }
                if (mapper == null) return default(CommonPageViewModel<T2>);


                destination = new CommonPageViewModel<T2>();
                destination.Status = source.Status;
                destination.StatusCode = source.StatusCode;
                destination.Msg = source.Message;
                destination.ResultData = mapper.Map<T, T2>(source.ResultData);
                destination.TotalCount = source.TotalCount;
                return destination;
            }


        }



    }
}
