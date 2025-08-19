using API.Entities.Users;

namespace API.Entities.JWT;

public class JWTRefreshToken : BaseEntity
{
    public string Token { get; set; }
    public DateTime ExpiredAt { get; set; }
    public bool IsRevoked { get; set; }
    public string UserId { get; set; }
    public UserCustom User { get; set; }
}