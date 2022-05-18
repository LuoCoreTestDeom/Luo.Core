using AutoMapper;
using Luo.Core.FiltersExtend.PolicysHandlers;
using Luo.Core.IServices;
using Luo.Core.Models.ViewModels.Request;
using Luo.Core.Utility.JsonWebToken;
using Luo.Core.Utility.JsonWebToken.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Luo.Core.Api.Controllers
{
    public class SecretController : ControllerBase
    {
        readonly IJwtAppService _jwtService;
        readonly IMemberService _service;
         readonly IMapper _Mapper;
        public SecretController(IJwtAppService jwtService, IMemberService service, IMapper mapper)
        {
            _jwtService = jwtService;
            _service = service;
            _Mapper = mapper;
        }
        /// <summary>
        /// 停用 Jwt 授权数据
        /// </summary>
        /// <returns></returns>
        [HttpPost("deactivate")]
        public async Task<IActionResult> CancelAccessToken()
        {
            await _jwtService.DeactivateCurrentAsync();
            return Ok();
        }
        /// <summary>
        /// 获取 Jwt 授权数据
        /// </summary>
        /// <param name="dto">授权用户信息</param>
        [HttpPost("token")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] SecretDto req)
        {
            JwtResponseDto res = new JwtResponseDto() { Type = "Bearer" };
           var resData= _service.JwtQueryMemberInfo(_Mapper.Map<JwtMemberInfoQuery>(req));
            if (resData.Status) 
            {
                res.Access = "账号密码不正确，无权访问。" + resData.Msg;
                return Ok(res);
            }
            var permission = new PermissionItem()
            {
                Role = resData.ResultData.MemberId.ToString(),
                Url = resData.ResultData.MemberName
            };

            var jwt = _jwtService.Create(permission);

            return Ok(new JwtResponseDto
            {
                Access = jwt.Token,
                Type = "Bearer",
                Profile = new JwtProfile
                {
                    Name = permission.Role,
                    Auths = jwt.Auths,
                    Expires = jwt.Expires
                }
            });
        }


        /// <summary>
        /// 刷新 Jwt 授权数据
        /// </summary>
        /// <param name="dto">刷新授权用户信息</param>
        /// <returns></returns>
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshAccessTokenAsync([FromBody]SecretDto dto)
        {
            //Todo：获取用户信息
            var permission = new PermissionItem()
            {
                Role = "Luo",
                Url = "luocore.com"
            };

            if (permission == null)
                return Ok(new JwtResponseDto
                {
                    Access = "无权访问",
                    Type = "Bearer",
                    Profile = new JwtProfile
                    {
                        Name = dto.Account,
                        Auths = 0,
                        Expires = 0
                    }
                });

            var jwt = await _jwtService.RefreshAsync(dto.Token, permission);

            return Ok(new JwtResponseDto
            {
                Access = jwt.Token,
                Type = "Bearer",
                Profile = new JwtProfile
                {
                    Name = permission.Role,
                    Auths = jwt.Success ? jwt.Auths : 0,
                    Expires = jwt.Success ? jwt.Expires : 0
                }
            });
        }

    }
}
