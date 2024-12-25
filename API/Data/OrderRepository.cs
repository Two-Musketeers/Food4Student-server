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
            .Include(o => o.Restaurant) // Include Restaurant
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId)
    {
        return await context.Orders
            .Where(o => o.AppUserId == userId)
            .Include(o => o.Restaurant)
                .ThenInclude(r => r.Logo)
            .Include(o => o.OrderItems)
            .ToListAsync();
    }

    public async Task<IEnumerable<Order>> GetOrdersByRestaurantIdAsync(string restaurantId)
    {
        return await context.Orders
            .Where(o => o.RestaurantId == restaurantId)
            .Include(o => o.Restaurant)
                .ThenInclude(r => r.Logo)
            .Include(o => o.OrderItems)
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

    public async Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status)
    {
        return await context.Orders
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.OriginalFoodItem)
                    .ThenInclude(fi => fi.FoodItemPhoto)
            .Include(o => o.Restaurant)
                .ThenInclude(r => r.Logo)
            .Where(o => o.Status == status)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();
    }
}
