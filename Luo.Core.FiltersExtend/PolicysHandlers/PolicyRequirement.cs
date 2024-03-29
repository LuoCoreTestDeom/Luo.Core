﻿
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace Luo.Core.FiltersExtend.PolicysHandlers
{
    /// <summary>
    /// 自定义策略要求
    /// 必要参数类，类似一个订单信息
    /// 继承 IAuthorizationRequirement，用于设计自定义权限处理器 PolicyHandler
    /// 因为AuthorizationHandler 中的泛型参数 TRequirement 必须继承 IAuthorizationRequirement
    /// </summary>
    public class PolicyRequirement : IAuthorizationRequirement
    {
        /// <summary>
        /// 用户权限集合，一个订单包含了很多详情，
        /// 同理，一个网站的认证发行中，也有很多权限详情(这里是Role和URL的关系)
        /// </summary>
        public List<PermissionItem> Permissions { get; set; }
     

        /// <summary>
        /// 认证授权类型
        /// </summary>
        public string ClaimType { get; set; }
       
        /// <summary>
        /// 签名验证
        /// </summary>
        public SigningCredentials SigningCredentials { get; set; }


        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="deniedAction">拒约请求的url</param>
        /// <param name="permissions">权限集合</param>
        /// <param name="claimType">声明类型</param>
        /// <param name="issuer">发行人</param>
        /// <param name="audience">订阅人</param>
        /// <param name="signingCredentials">签名验证实体</param>
        /// <param name="expiration">过期时间</param>
        public PolicyRequirement( List<PermissionItem> permissions, string claimType, SigningCredentials signingCredentials)
        {
            ClaimType = claimType;
            Permissions = permissions;
            SigningCredentials = signingCredentials;
        }
    }
}
