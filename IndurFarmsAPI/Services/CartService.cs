using IndurFarmsAPI.DTO_s;
using IndurFarmsAPI.IRepositories;
using IndurFarmsAPI.IServices;
using IndurFarmsAPI.Models;

namespace IndurFarmsAPI.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICurrentUserService _currentUserService;
        public CartService(ICartRepository cartRepository, ICurrentUserService currentUserService) 
        {
            _cartRepository = cartRepository;
            _currentUserService = currentUserService;
        }

        public async Task<ICollection<GetCartItemsDto>> GetUserCartItems()
        {
            CurrentUserDto currentUserDto = await _currentUserService.GetCurrentUserAsync();
            ICollection<CartItems> cartItems = await _cartRepository.GetUserCartItemsAsync(currentUserDto.UserId);
            return cartItems.Select(item=> new GetCartItemsDto
            {
                Id = item.Id,
                ProductId = item.ProductId,
                Quantity = item.Quantity
            }).ToList();
        }
        public async Task<GetCartItemsDto> AddToCartAsync(AddToCartDto cartItems)
        {
            CurrentUserDto currentUserDto = await _currentUserService.GetCurrentUserAsync();
            int? cartId = await _cartRepository.GetCartId(currentUserDto.UserId);
            if(cartId == null)
            {
                Cart cart = new Cart{
                    UserId = currentUserDto.UserId
                };
                // Add user to Cart 
                await _cartRepository.AddUserToCart(cart);
                int? currentCartId = await _cartRepository.GetCartId(currentUserDto.UserId);
                return await AddProductToCart(currentCartId, cartItems);
            }
            CartItems cartItem = await _cartRepository.GetItemByIdAsync(cartId, cartItems.ProductId);
            if (cartItem == null)
            {
                return await AddProductToCart(cartId, cartItems);
            }
            else
            {
                cartItem.Quantity += cartItems.Quantity;
                await _cartRepository.UpdateCartItemAsync(cartItem);
                return new GetCartItemsDto
                {
                    Id = cartItem.Id,
                    ProductId = cartItem.ProductId,
                    Quantity = cartItems.Quantity
                };
            }
        }

        public async Task<GetCartItemsDto> UpdateCartProductAsync(GetCartItemsDto getCartItemsDto)
        {
            CurrentUserDto currentUserDto = await _currentUserService.GetCurrentUserAsync();
            CartItems cartItem = await _cartRepository.GetCartItemAsync(currentUserDto.UserId, getCartItemsDto.ProductId);
            if(getCartItemsDto.Quantity > 0)
            {
                cartItem.Quantity = getCartItemsDto.Quantity;
                await _cartRepository.UpdateCartItemAsync(cartItem);
                return new GetCartItemsDto
                {
                    Id = cartItem.Id,
                    ProductId = cartItem.ProductId,
                    Quantity = getCartItemsDto.Quantity
                };
            }
            else
            {
                await _cartRepository.DeleteCartItemAsync(cartItem);
                return new GetCartItemsDto { };
            }
        }

        public async Task DeleteProductAsync(GetCartItemsDto getCartItemsDto)
        {
            CurrentUserDto currentUserDto = await _currentUserService.GetCurrentUserAsync();
            CartItems cartItem = await _cartRepository.GetCartItemAsync(currentUserDto.UserId, getCartItemsDto.ProductId);
            await _cartRepository.DeleteCartItemAsync(cartItem);
        }

        private async Task<GetCartItemsDto> AddProductToCart(int? cartId, AddToCartDto cartItems)
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
    }
}