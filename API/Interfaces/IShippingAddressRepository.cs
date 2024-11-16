using API.Entities;

namespace API.Interfaces;

public interface IShippingAddressRepository
{
    void AddShippingAddress(ShippingAddress shippingAddress);
    Task<ShippingAddress> GetShippingAddressByIdAsync(int id);
    void DeleteShippingAddress(ShippingAddress shippingAddress);
    Task<bool> SaveAllAsync();
}
