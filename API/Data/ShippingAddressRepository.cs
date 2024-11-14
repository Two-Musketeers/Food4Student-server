using API.Entities;
using API.Interfaces;

namespace API.Data;

public class ShippingAddressRepository(DataContext context) : IShippingAddressRepository
{
    public async Task AddShippingAddressAsync(ShippingAddress shippingAddress)
    {
        context.ShippingAddresses.Add(shippingAddress);
        await context.SaveChangesAsync();
    }
}
