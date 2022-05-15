using Luo.Core.EnumModels;
using Luo.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luo.Core.FiltersExtend.AuthorizationFilters
{
    public class AuthResponse
    {

        public int Status { get; set; } = 200;
        public string Value { get; set; } = "";
        public ApiCommonResult<string> _result = new ApiCommonResult<string>() { };

        public AuthResponse(ResponseHttpStatusCode apiCode, string msg = null)
        {
            switch (apiCode)
            {
                case ResponseHttpStatusCode.CODE304:
                    {
                        Status = 304;
                        Value = "缓存资源已清除，请重新登录!";
                    }
                    break;
                case ResponseHttpStatusCode.CODE401:
                    {
                        Status = 401;
                        Value = "很抱歉，您无权访问该接口，请确保已经登录!";
                    }
                    break;
                case ResponseHttpStatusCode.CODE403:
                    {
                        Status = 403;
                        Value = "很抱歉，您的访问权限等级不够，联系管理员!";
                    }
                    break;
                case ResponseHttpStatusCode.CODE404:
                    {
                        Status = 404;
                        Value = "资源不存在!";
                    }
                    break;
                case ResponseHttpStatusCode.CODE500:
                    {
                        Status = 500;
                        Value = msg;
                    }
                    break;
            }

            _result = new ApiCommonResult<string>()
            {
                Status = Status,
                Msg = Value,
                State = apiCode != ResponseHttpStatusCode.CODE200
            };
        }
    }
}
