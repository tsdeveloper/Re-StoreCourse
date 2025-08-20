using API.DTOs.Addresses;
using API.Entities.Aggregate;

namespace API.DTOs.ShippingAddresses;

public class ShippingAddressDto : AddressDto
{
    public static explicit operator ShippingAddressDto(ShippingAddress address)
    {
        return new ShippingAddressDto
        {
            FullName = address.FullName,
            Address1 = address.Address1,
            Address2 = address.Address2,
            City = address.City,
            State = address.State,
            Zip = address.Zip,
            Country = address.Country,

        };
    }
}