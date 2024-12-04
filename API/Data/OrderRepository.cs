using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class OrderRepository(DataContext context) : IOrderRepository
{
    public void AddOrder(Order order)
    {
        context.Orders.Add(order);
    }

    public void DeleteOrder(Order order)
    {
        context.Orders.Remove(order);
    }

    public async Task<Order?> GetOrderByIdAsync(string id)
    {
        return await context.Orders
            .Include(o => o.ShippingAddress)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.FoodItemPhoto)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.OrderItemVariations)
                    .ThenInclude(oiv => oiv.Variation)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.OrderItemVariations)
                    .ThenInclude(oiv => oiv.VariationOption)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId)
    {
        return await context.Orders
            .Where(o => o.AppUserId == userId)
            .Include(o => o.ShippingAddress)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.FoodItemPhoto)
            .ToListAsync();
    }

    public async Task<IEnumerable<Order>> GetOrdersByRestaurantIdAsync(string restaurantId)
    {
        return await context.Orders
            .Where(o => o.RestaurantId == restaurantId)
            .Include(o => o.ShippingAddress)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.FoodItemPhoto)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.OrderItemVariations)
                    .ThenInclude(oiv => oiv.Variation)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.OrderItemVariations)
                    .ThenInclude(oiv => oiv.VariationOption)
            .ToListAsync();
    }

    public async Task<bool> SaveAllAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }

    public void UpdateOrder(Order order)
    {
        context.Orders.Update(order);
    }
}
