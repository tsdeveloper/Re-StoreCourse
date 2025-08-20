using API.Entities.Addresses;

namespace API.Entities.Users;

public class UserAddress : Address
{
    public int UserId { get; set; }
    public UserCustom User { get; set; }
}