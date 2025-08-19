namespace API.DTOs;

public class UserLoginDto
{
    public string Email { get; set; }
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public BasketReturnDTO Basket { get; set; }
}