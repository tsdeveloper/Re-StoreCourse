namespace API.Entities.JWT;

public class JWTSettings
{
    public string TokenKey { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public double ExpireMinute { get; set; }
    public double ExpireRefreshTokenMinute { get; set; }
}