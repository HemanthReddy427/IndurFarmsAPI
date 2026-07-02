using IndurFarmsAPI.IRepositories;
using IndurFarmsAPI.Middleware;
using IndurFarmsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace IndurFarmsAPI.Repositories
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly IndurFarmsDbContext _indurFarmsDbContext;
        public OrdersRepository(IndurFarmsDbContext indurFarmsDbContext)
        {
            _indurFarmsDbContext = indurFarmsDbContext;
        }

        public async Task ClearCartAsync(ICollection<CartItems> cartItems)
        {
            _indurFarmsDbContext.RemoveRange(cartItems);
            await _indurFarmsDbContext.SaveChangesAsync();
        }

        public async Task<Orders> CreateOrderAsync(Orders order)
        {
            await _indurFarmsDbContext.Orders.AddAsync(order);
            await _indurFarmsDbContext.SaveChangesAsync();
            return order;
        }

        public async Task CreateOrderItemsAsync(ICollection<OrderItems> orderItems)
        {
            await _indurFarmsDbContext.AddRangeAsync(orderItems);
            await _indurFarmsDbContext.SaveChangesAsync();
        }

        public async Task<ICollection<OrderItems>> GetOrderItemsAsync(int orderId)
        {
            return await _indurFarmsDbContext.OrderItems.Where(ordItem=> ordItem.OrderId == orderId).ToListAsync();
        }

        public async Task<ICollection<Orders>> GetAllOrdersForUser(int userId)
        {
            ICollection<Orders> orders = await _indurFarmsDbContext.Orders.Where(ord=>ord.UserId == userId).ToListAsync();
            return orders;
        }

        public async Task UpdateOrderStatus(Orders order, int orderStatus)
        {
            order.OrderStatus = orderStatus;
            await _indurFarmsDbContext.SaveChangesAsync();
        }
    }
}
