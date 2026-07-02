using IndurFarmsAPI.Models;

namespace IndurFarmsAPI.IRepositories
{
    public interface IOrdersRepository
    {
        Task<Orders> CreateOrderAsync(Orders orders);
        Task CreateOrderItemsAsync(ICollection<OrderItems> orderItems);
        Task<ICollection<OrderItems>> GetOrderItemsAsync(int orderId);
        Task<ICollection<Orders>> GetAllOrdersForUser(int userId);
        Task ClearCartAsync(ICollection<CartItems> cartItems);
        Task UpdateOrderStatus(Orders order, int orderStatus);
    }
}
