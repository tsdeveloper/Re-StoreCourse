using API.DTOs.Addresses;
using API.DTOs.Baskets;

namespace API.DTOs.Users;

public class UserLoginDto
{
    public string Email { get; set; }
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public BasketDTO Basket { get; set; }
}

