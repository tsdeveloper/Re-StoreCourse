using API.Entities;
using API.Entities.Addresses;

namespace API.DTOs.Addresses;

public class AddressDto : BaseEntity
{
    public string FullName { get; set; }
    public string Address1 { get; set; }
    public string Address2 { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Zip { get; set; }
    public string Country { get; set; }

    public static explicit operator AddressDto(Address entity)
    {
        return new AddressDto
        {
            FullName = entity.FullName,
            Address1 = entity.Address1,
            Address2 = entity.Address2,
            City = entity.City,
            State = entity.State,
            Zip = entity.Zip,
            Country = entity.Country,
        };
    }
}