using API.Entities;

namespace API.Interfaces;

public interface IOrderRepository
{
    void AddOrder(Order order);
    void UpdateOrder(Order order);
    void DeleteOrder(Order order);
    Task<Order?> GetOrderByIdAsync(string id);
    Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId);
    Task<IEnumerable<Order>> GetOrdersByRestaurantIdAsync(string restaurantId);
    Task<bool> SaveAllAsync();
}
