using API.DTOs.ShippingAddresses;
using API.Entities.Addresses;
using Microsoft.EntityFrameworkCore;

namespace API.Entities.Aggregate;

[Owned]
public class ShippingAddress : Address
{
    public static explicit operator ShippingAddress(ShippingAddressDto address)
    {
        return new ShippingAddress
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