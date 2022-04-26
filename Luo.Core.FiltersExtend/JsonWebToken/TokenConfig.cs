namespace Luo.Core.FiltersExtend.JsonWebToken;

public class TokenConfig
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string SecurityKey { get; set; }
    public int AccessExpiration { get; set; }
    public int RefreshExpiration { get; set; }
}
