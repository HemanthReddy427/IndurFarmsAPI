using IndurFarmsAPI.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IndurFarmsAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class OrdersController : Controller
    {
        private readonly IOrdersService _orderService;
        public OrdersController(IOrdersService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("PlaceOrder/{addressId}")]
        public async Task<IActionResult> PlaceOrderAsync([FromRoute]int addressId)
        {
            var response = await _orderService.PlaceOrderAsync(addressId);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetOrdersForUser()
        {
            return Ok(await _orderService.GetAllUserOrders());
        }

        [HttpGet("{orderId:int}")]
        public async Task<IActionResult> GetOrderItems([FromRoute]int orderId)
        {
            return Ok(await _orderService.GetOrderItemsAsync(orderId));
        }

        [HttpDelete("{orderId}")]
        public async Task<IActionResult> CancelOrder([FromRoute]int orderId)
        {
            await _orderService.CancelOrder(orderId);
            return Ok("Order cancelled Successfully");
        }

        [HttpGet("AllOrders")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllOrders()
        {
            return Ok(await _orderService.GetOrdersAsync());
        }

        [HttpPut("{orderId}/Status/{status}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> UpdateOrderStatus([FromRoute]int orderId,[FromRoute] int status)
        {
            await _orderService.UpdateOrderStatus(orderId, status);
            return Ok("Order updated successfully");
        }
    }
}
