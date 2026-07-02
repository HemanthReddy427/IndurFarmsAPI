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

        [HttpPost]
        public async Task<IActionResult> PlaceOrderAsync(int addressId)
        {
            await _orderService.PlaceOrderAsync(addressId);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetOrdersForUser()
        {
            return Ok(await _orderService.GetAllUserOrders());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderItems([FromRoute]int orderId)
        {
            return Ok(await _orderService.GetOrderItemsAsync(orderId));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelOrder([FromRoute]int orderId)
        {
            await _orderService.CancelOrder(orderId);
            return Ok("Order cancelled Successfully");
        }
    }
}
