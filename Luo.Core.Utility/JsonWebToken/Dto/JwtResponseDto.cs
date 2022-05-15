using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luo.Core.Utility.JsonWebToken.Dto;

public class JwtResponseDto
{
    /// <summary>
    /// 访问 Token 值
    /// </summary>
    public string Access { get; set; }

    /// <summary>
    /// 授权类型
    /// </summary>
    public string Type { get; set; } = "Bearer";

    /// <summary>
    /// 个人信息
    /// </summary>
    public JwtProfile Profile { get; set; }
}

/// <summary>
/// 个人信息
/// </summary>
public class JwtProfile
{
    /// <summary>
    /// 姓名
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 授权时间戳
    /// </summary>
    public long Auths { get; set; }

    /// <summary>
    /// 过期时间戳
    /// </summary>
    public long Expires { get; set; }
}
