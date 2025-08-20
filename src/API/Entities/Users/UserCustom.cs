using API.Entities.JWT;
using Microsoft.AspNetCore.Identity;

namespace API.Entities.Users;

public class UserCustom : IdentityUser<int>
{
    public UserAddress Address { get; set; }
    public ICollection<JWTRefreshToken> RefreshTokenList { get; set; } = new List<JWTRefreshToken>();
}