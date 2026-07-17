using IndurFarmsAPI.DTO_s;
using IndurFarmsAPI.Models;

namespace IndurFarmsAPI.IServices
{
    public interface IOrdersService
    {
        Task<PlaceOrderResponseDto> PlaceOrderAsync(int addressId);
        Task<ICollection<PlaceOrderResponseDto>> GetAllUserOrders();
        Task<ICollection<OrderItemResponseDto>> GetOrderItemsAsync(int orderId);

        Task CancelOrder(int orderId);
        Task<ICollection<Orders>> GetOrdersAsync();
        Task UpdateOrderStatus(int orderId, int status);
    }
}
