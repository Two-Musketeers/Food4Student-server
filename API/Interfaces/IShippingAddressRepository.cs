using API.Entities;

namespace API.Interfaces;

public interface IShippingAddressRepository
{
    void AddShippingAddress(ShippingAddress shippingAddress);
    Task<ShippingAddress> GetShippingAddressByIdAsync(string id);
    void DeleteShippingAddress(ShippingAddress shippingAddress);
    Task<bool> SaveAllAsync();
}
