using IndurFarmsAPI.DTO_s;
using IndurFarmsAPI.IRepositories;
using IndurFarmsAPI.IServices;
using IndurFarmsAPI.Middleware;
using IndurFarmsAPI.Models;

namespace IndurFarmsAPI.Services
{
    public class CartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICurrentUserService _currentUserService;
        public CartService(ICartRepository cartRepository) 
        {
            _cartRepository = cartRepository;
        }

        public async Task<GetCartItemsDto> AddToCartAsync(AddToCartDto cartItems)
        {
            CurrentUserDto currentUserDto = await _currentUserService.GetCurrentUserAsync();
            int cartId = await _cartRepository.GetCartId(currentUserDto.UserId);
            CartItems cartItem = await _cartRepository.GetItemByIdAsync(cartId, currentUserDto.UserId);
            if (cartItem == null)
            {
                CartItems cartItems1 = new CartItems
                {
                    CartId = cartId,
                    ProductId = cartItems.ProductId,
                    Quantity = cartItems.Quantity
                };
                await _cartRepository.AddToCartAsync(cartItems1);
                return new GetCartItemsDto
                {
                    ProductId = cartItems1.ProductId,
                    Quantity = cartItems1.Quantity
                };
            }
            else
            {
                cartItem.Quantity = cartItems.Quantity;
                await _cartRepository.UpdateCartItemAsync(cartItem);
                return new GetCartItemsDto
                {
                    Id = cartItem.Id,
                    ProductId = cartItem.ProductId,
                    Quantity = cartItems.Quantity
                };
            }
        }
    }
}
