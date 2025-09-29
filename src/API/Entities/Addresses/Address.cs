using API.DTOs.ShippingAddresses;

namespace API.Entities.Addresses;

public class Address : BaseEntity
{
    public string FullName { get; set; }
    public string Address1 { get; set; }
    public string Address2 { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Zip { get; set; }
    public string Country { get; set; }

    public static explicit operator Address(ShippingAddressDto address)
    {
        return new Address
        {
            FullName = address.FullName,
            Address1 = address.Address1,
            Address2 = address.Address2,
            City = address.City,
            State = address.State,
            Zip = address.Zip,
            Country = address.Country
        };
    }
}