namespace API.Entities.JWT;

public class JWTSettings
{
    public string TokenKey { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public double Expire { get; set; }
    public double ExpireRefreshToken { get; set; }
}