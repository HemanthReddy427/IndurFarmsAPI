using IndurFarmsAPI.DTO_s;
using IndurFarmsAPI.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IndurFarmsAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        [Route("GetAllItems")]
        public async Task<IActionResult> GetUserCartItems()
        {
            ICollection<GetCartItemsDto> getCartItemsDtos = await _cartService.GetUserCartItems();
            return Ok(getCartItemsDtos);
        }

        [HttpPost]
        [Route("AddItem")]
        public async Task<IActionResult> AddItemToCart(AddToCartDto addToCartDto)
        {
            GetCartItemsDto getCartItemsDto = await _cartService.AddToCartAsync(addToCartDto);
            return Ok(getCartItemsDto);
        }

        [HttpPost]
        [Route("UpdateItem")]
        public async Task<IActionResult> UpdateCartItemAsync(GetCartItemsDto getCartItemsDto)
        {
            GetCartItemsDto getUpdatedItem = await _cartService.UpdateCartProductAsync(getCartItemsDto);
            return Ok(getUpdatedItem);
        }

        [HttpDelete]
        [Route("DeleteItem")]
        public async Task<IActionResult> DeleteProductAsync(GetCartItemsDto getCartItemsDto)
        {
            await _cartService.DeleteProductAsync(getCartItemsDto);
            return Ok("Product Deleted Successfullt");
        }
    }
}
