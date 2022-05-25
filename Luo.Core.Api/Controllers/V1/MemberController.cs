using AutoMapper;
using Luo.Core.FiltersExtend.PolicysHandlers;
using Luo.Core.IServices;
using Luo.Core.Models.ViewModels.Request;
using Luo.Core.Utility.JsonWebToken;
using Luo.Core.Utility.JsonWebToken.Dto;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Luo.Core.Api.Controllers.V1
{
    [ApiExplorerSettings(GroupName = "V1")]
    //[ApiVersion("1.0")]
    [Route("api/v1/[controller]/[action]")]
    //[Route("api/v{version:apiVersion}/[controller]/[action]")]
    [ApiController]
    [Authorize(Policy = "Permission")]
    //[Authorize]
    public class MemberController : ControllerBase
    {

        readonly IJwtAppService _jwtService;
        readonly IMemberService _service;
        readonly IMapper _Mapper;
        public MemberController(IJwtAppService jwtService, IMemberService service, IMapper mapper)
        {
            _jwtService = jwtService;
            _service = service;
            _Mapper = mapper;
        }


        /// <summary>
        /// 获取 Jwt 授权数据
        /// </summary>
        /// <param name="dto">授权用户信息</param>
        [HttpPost(Name ="Login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody]MemberLogin req)
        {
            JwtResponseDto res = new JwtResponseDto() { Type = "Bearer" };
            var resData = _service.LoginMemberInfo(req);
            if (!resData.Status)
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
        [HttpPost(Name ="RefreshMember")]
        public async Task<IActionResult> RefreshAccessToken([FromBody]MemberLogin req)
        {

            JwtResponseDto res = new JwtResponseDto() { Type = "Bearer" };
            var resData = _service.LoginMemberInfo(req);
            if (!resData.Status)
            {
                res.Access = "账号密码不正确，无权访问。" + resData.Msg;
                return Ok(res);
            }
            var permission = new PermissionItem()
            {
                Role = resData.ResultData.MemberId.ToString(),
                Url = resData.ResultData.MemberName
            };



            if (permission == null)
                return Ok(new JwtResponseDto
                {
                    Access = "无权访问",
                    Type = "Bearer",
                    Profile = new JwtProfile
                    {
                        Name = req.Account,
                        Auths = 0,
                        Expires = 0
                    }
                });

            var jwt = await _jwtService.RefreshAsync(req.Token, permission);

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
