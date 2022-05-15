using Luo.Core.Common;

namespace Luo.Core.Utility.JsonWebToken;


public  class TokenConfig
{
    public static JsonWebTokenModel JwtData { get => Appsettings.GetObject<JsonWebTokenModel>("JsonWebToken"); }
}
public class JsonWebTokenModel
{
    public string Secret { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string DeniedAction { get; set; }
    public double ExpirationSeconds { get; set; }
}


