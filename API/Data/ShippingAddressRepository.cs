using API.Entities;
using API.Interfaces;

namespace API.Data;

public class ShippingAddressRepository(DataContext context) : IShippingAddressRepository
{
    public async Task<ShippingAddress> GetShippingAddressByIdAsync(string id)
    {

        var shippingAddress = await context.ShippingAddresses.FindAsync(id) ?? throw new Exception("Shipping Address not found");
        return shippingAddress;
    }

    public void AddShippingAddress(ShippingAddress shippingAddress)
    {
        context.ShippingAddresses.Add(shippingAddress);
    }
    
    public void DeleteShippingAddress(ShippingAddress shippingAddress)
    {
        context.ShippingAddresses.Remove(shippingAddress);
    }

    public async Task<bool> SaveAllAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }
}
