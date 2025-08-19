using API.Entities.JWT;
using Microsoft.AspNetCore.Identity;

namespace API.Entities.Users;

public class UserCustom : IdentityUser
{
    public ICollection<JWTRefreshToken> RefreshTokenList { get; set; } = new List<JWTRefreshToken>();
}