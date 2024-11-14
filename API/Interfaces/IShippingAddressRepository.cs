using API.Entities;

namespace API.Interfaces;

public interface IShippingAddressRepository
{
    Task AddShippingAddressAsync(ShippingAddress shippingAddress);
}
